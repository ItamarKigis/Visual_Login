using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
namespace Navigation_Drawer_App
{
    /// <summary>
    /// Interaction logic for UserLoginSequence.xaml
    /// </summary>
    public partial class UserLoginSequence : Window
    {
        private Polyline NewPolyline = null;
        private Polygon currPolygon = null;
        List<Point> edgePoints = new List<Point>();
        Menu obj = new Menu();
        int pointAccuracy;
        public UserLoginSequence(string EdgePoints, string img, double x, double y, double w, double h)
        {
            pointAccuracy = Menu.pointAccuracy;
            InitializeComponent();
            parseEdgePoints(EdgePoints);
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(img, UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.DecodePixelWidth = int.Parse(Menu.width.ToString());
            src.DecodePixelHeight = int.Parse(Menu.height.ToString()) ;
            src.EndInit();
            
            CroppedBitmap Object = new CroppedBitmap
                (src, new Int32Rect((int)x - (int)((670.0 - Menu.width)/2), (int)y - (int)((465.0 - Menu.height) / 2), (int)w , (int)h));
            imgPhoto.Source = Object;
        }
        private void parseEdgePoints(string EdgePoints)
        {
            string[] words = EdgePoints.Split(')');          
            foreach (var word in words)
            {
                if(word == " ")
                {
                    return;
                }
                string[] cords = word.Split(',');

                string x = cords[0];
                x = x.Substring(1, x.Length - 1);
                if(x[0] == '(')
                {
                    x = x.Substring(1, x.Length - 1);
                }
                string y = cords[1];
                y = y.Substring(1, y.Length - 1);
                edgePoints.Add(new Point(int.Parse(x), int.Parse(y)));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = Brushes.Sienna;
            ellipse.Width = 100;
            ellipse.Height = 100;
            ellipse.StrokeThickness = 2;

            canDraw.Children.Add(ellipse);

            Canvas.SetLeft(ellipse, e.GetPosition(imgPhoto).X);
            Canvas.SetTop(ellipse, e.GetPosition(imgPhoto).Y);
        }
        private void canDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (NewPolyline == null) return;
            NewPolyline.Points[NewPolyline.Points.Count - 1] =
                e.GetPosition(canDraw);
        }
        private void canDraw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // See which button was pressed.
            if (e.RightButton == MouseButtonState.Pressed)
            {
                // See if we are drawing a new polygon.
                if (NewPolyline != null)
                {
                    if (NewPolyline.Points.Count > 3)
                    {
                        // Remove the last point.
                        NewPolyline.Points.RemoveAt(NewPolyline.Points.Count - 1);

                        // Convert the new polyline into a polygon.
                        Polygon new_polygon = new Polygon();
                        new_polygon.Stroke = Brushes.Blue;
                        new_polygon.StrokeThickness = 2;
                        new_polygon.Points = NewPolyline.Points;

                        canDraw.Children.Remove(currPolygon);
                        canDraw.Children.Add(new_polygon);
                        currPolygon = new_polygon;

                        SubmitImage.IsEnabled = true;

                    }
                    canDraw.Children.Remove(NewPolyline);

                    NewPolyline = null;
                }
                return;
            }

            // If we don't have a new polygon, start one.
            if (NewPolyline == null)
            {
                // We have no new polygon. Start one.
                NewPolyline = new Polyline();
                NewPolyline.Stroke = Brushes.Red;
                NewPolyline.StrokeThickness = 1;
                NewPolyline.StrokeDashArray = new DoubleCollection();
                NewPolyline.StrokeDashArray.Add(5);
                NewPolyline.StrokeDashArray.Add(5);
                NewPolyline.Points.Add(e.GetPosition(canDraw));
                canDraw.Children.Add(NewPolyline);
            }

            // Add a point to the new polygon.
            NewPolyline.Points.Add(e.GetPosition(canDraw));
        }

        private void SubmitImage_Click(object sender, RoutedEventArgs e)
        {
            if(checkPolygonEdgePoints() == false)
            {
                EdgeErrorMsg.Visibility = Visibility.Visible;
                return;
            }
            List<Point> edgeData = new List<Point>();
            for (int i = 0; i < currPolygon.Points.Count; i++)
            {
                edgeData.Add(currPolygon.Points[i]);
            }
            string json = JsonSerializer.Serialize(edgeData);

            string startupPath = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo newPath = System.IO.Directory.GetParent(startupPath);
            newPath = System.IO.Directory.GetParent(newPath.FullName);

            string fileName = newPath.FullName + "\\JsonEdgeData.json";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(json);
            }
            this.Close();
        }
        private bool checkPolygonEdgePoints()
        {
            bool isPointValid = false;
            for(int i = 0; i < currPolygon.Points.Count; i++)
            {
                isPointValid = false;
                for (int j = 0; j < edgePoints.Count; j++)
                {
                    if(Math.Abs(currPolygon.Points[i].X - edgePoints[j].X) <= pointAccuracy && 
                        Math.Abs(currPolygon.Points[i].Y - edgePoints[j].Y) <= pointAccuracy)
                    {
                        isPointValid = true;
                        j = edgePoints.Count;
                    }
                }
                if(isPointValid == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
