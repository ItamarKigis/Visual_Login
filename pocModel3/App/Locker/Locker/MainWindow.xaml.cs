using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Locker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class SimplePoint
        {
            public int x;
            public int y;
        }
        private Polyline NewPolyline = null;
        private Polygon currPolygon = null;
        private List<SimplePoint> PolygonEdges;
        private int accuracy;
        public MainWindow()
        {
            InitializeComponent();
            


            string ch = "C:\\Users\\משתמש\\Documents\\pocModel3\\Images\\image\\img.jpg";
            BitmapImage image = new BitmapImage(new Uri(ch));
            LoginImage.Source = image;

            StreamReader r = new StreamReader("C:\\Users\\משתמש\\Documents\\pocModel3\\PolygonData.json");
            string json = r.ReadToEnd();
            PolygonEdges = JsonConvert.DeserializeObject<List<SimplePoint>>(json);
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

                        Submit.IsEnabled = true;

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

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            bool isEqual = true;
            try
            {
                for (int i = 0; i < currPolygon.Points.Count; i++)
                {
                    if (Math.Abs(currPolygon.Points[i].X - PolygonEdges[i].x) > 30)
                    {
                        if (Math.Abs(currPolygon.Points[i].Y - PolygonEdges[i].y) > 30)
                        {
                            isEqual = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isEqual = false;
            }
            if (!isEqual)
            {
                ErrorMsg.Visibility = Visibility.Visible;
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
        private void ForgotPassClick(object sender, RequestNavigateEventArgs e)
        {
            this.Content = new ForgotPass();
        }
    }
}
