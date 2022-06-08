using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using IronPython.Hosting;
using System.Text;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

using Bitmap = System.Drawing.Bitmap;

namespace Navigation_Drawer_App
{
    /// <summary>
    /// Interaction logic for AddImage.xaml
    /// </summary>
    class SimplePoint
    {
        public int x;
        public int y;
    }
    public partial class AddImage : UserControl
    {
        int[] x = new int[20];
        int[] y = new int[20];
        int[] w = new int[20];
        int[] h = new int[20];
        int originalHeight = 0;
        int originalWidth = 0;
        int NumOfObjects = 0;
        double widthOfImage = 0;
        double heightOfImage = 0;
        string ImgPath;
        //(px,py) (px,py) .....
        string edgePoints;
        //points of polygon
        List<SimplePoint> edgesPolygon;
        //Polygon object to draw
        Polygon polygon;
        public AddImage()
        {
            Menu obj = new Menu();
            widthOfImage = Menu.width;
            heightOfImage = Menu.height;

            InitializeComponent();
            this.DataContext = this;
            ImagePlace.Width = widthOfImage;
            ImagePlace.Height = heightOfImage;
        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
                imgPhoto.Width = Menu.width;
                imgPhoto.Height = Menu.height;
                canvas.Children.Clear();
                this.grit.Children.Remove(Plus);
                this.grit.Children.Remove(Folder);
                ImgPath = op.FileName;
                run_cmd(@"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python36_64\python.exe",
                    "C:\\Users\\משתמש\\Documents\\pocModel3\\yolo_object_detection.py " + op.FileName);

                Thread tr = new Thread(new ThreadStart(drawRectangles));
                tr.Start();

                InstructionText.Text = "Choose one of the objects below to make a visual login: ";
                anotherImage.Text = "(or pick another image)";
            }
        }
        private void run_cmd(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = cmd;//cmd is full path to python.exe
            start.Arguments = args;//args is path to .py file and any cmd line args
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    string MaybeError = process.StandardError.ReadToEnd();
                    GetDataOfObjects(result);
                }
            }
        }
        private void GetDataOfObjects(string result)
        {
            char[] delimiterChars = { '\r', '\n'};
            string[] placements = result.Split(delimiterChars);

            string orignalShape = placements[0];
            placements = placements.Where(w => w != placements[0]).ToArray();
            foreach (string place in placements)
            {
                if(place != "")
                {
                    x[NumOfObjects] = Int32.Parse(place.Split(' ')[0]);
                    y[NumOfObjects] = Int32.Parse(place.Split(' ')[1]); 
                    w[NumOfObjects] = Int32.Parse(place.Split(' ')[2]); 
                    h[NumOfObjects] = Int32.Parse(place.Split(' ')[3]);
                    NumOfObjects++;
                }
            }
            originalWidth = Int32.Parse(orignalShape.Split(' ')[0]);
            originalHeight = Int32.Parse(orignalShape.Split(' ')[1]);
        }
        private void drawRectangles()
        {
            double imageHeightRatio = 0;
            double imageWidthRatio = 0;
            imageHeightRatio = heightOfImage / originalHeight;
            imageWidthRatio = widthOfImage / originalWidth;

            Thread.Sleep(1000);
            for(int i = 0; i < NumOfObjects; i++)
            
            
            {
                this.Dispatcher.Invoke(() => {
                    Rectangle rect = new Rectangle()
                    {
                    Width = w[i] * imageWidthRatio,
                    Height = h[i] * imageHeightRatio,
                    Fill = null,
                    Stroke = Brushes.Red,
                    StrokeThickness = 2,
                    
                };
                canvas.Children.Add(rect);
                Canvas.SetTop(rect, y[i] * imageHeightRatio + (465 - Menu.height) / 2); // y of rectangle
                Canvas.SetLeft(rect, x[i] * imageWidthRatio + (670 - Menu.width) / 2 ); // x of rectangle
                
                });
            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas);
            foreach (Rectangle rect in canvas.Children)
            {
                double X = Canvas.GetLeft((UIElement)rect);
                double Y = Canvas.GetTop((UIElement)rect);
                double W = rect.Width;
                double H = rect.Height;
                if (p.X >= X && X + W >= p.X)
                {
                    if (p.Y >= Y && Y + H >= p.Y)
                    {
                        runEdgeDetection(@"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python36_64\python.exe",
                    "C:\\Users\\משתמש\\Documents\\pocModel3\\edgeDetection.py " + ImgPath + " " + X + " " + Y +
                    " " + W + " " + H);

                        //Openning new window to draw the polygon

                        UserLoginSequence win2 = new UserLoginSequence(edgePoints, ImgPath, X,Y,W,H);
                        win2.ShowDialog();

                        string startupPath = System.IO.Directory.GetCurrentDirectory();
                        DirectoryInfo newPath = System.IO.Directory.GetParent(startupPath);
                        newPath = System.IO.Directory.GetParent(newPath.FullName);
                        StreamReader r = new StreamReader(newPath.FullName + "\\JsonEdgeData.json");
                        string json = r.ReadToEnd();
                        edgesPolygon = JsonConvert.DeserializeObject<List<SimplePoint>>(json);
                        
                        for(int i = 0; i < edgesPolygon.Count; i++)
                        {
                            //edgesPolygon.ElementAt(i).x = (int)(X + (edgesPolygon.ElementAt(i).x / widthOfImage) * W);
                            edgesPolygon.ElementAt(i).x = (int)(X + (int)((edgesPolygon.ElementAt(i).x) / 670.0 * W));

                            //edgesPolygon.ElementAt(i).y = (int)(Y + (edgesPolygon.ElementAt(i).y / heightOfImage) * H);
                            edgesPolygon.ElementAt(i).y = (int)(Y + (int)((edgesPolygon.ElementAt(i).y) / 465.0 * H));
                        }
                        SubmitPolygonOnImage();
                    }
                }
            }
        }

        private void canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas);
            foreach (Rectangle rect in canvas.Children)
            {
                double X = Canvas.GetLeft((UIElement)rect);
                double Y = Canvas.GetTop((UIElement)rect);
                double W = rect.Width;
                double H = rect.Height;
                if(p.X >= X && X + W >= p.X)
                {
                    if(p.Y >= Y && Y + H >= p.Y)
                    {
                        rect.Fill = Brushes.Lavender;
                        rect.Opacity = 0.25;
                    }
                }
            }
        }

        private void canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas);
            foreach (Rectangle rect in canvas.Children)
            {
                rect.Opacity = 1;
                rect.Fill = null;
            }
        }
        private void runEdgeDetection(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = cmd;//cmd is full path to python.exe
            start.Arguments = args;//args is path to .py file and any cmd line args
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    string MaybeError = process.StandardError.ReadToEnd();
                    edgePoints = result;
                }
            }
        }

        private void SubmitPolygonOnImage()
        {
            polygon = new Polygon();
            InstructionText.Visibility = Visibility.Hidden;
            Load.Visibility = Visibility.Hidden;
            UploadSymbol.Visibility = Visibility.Hidden;
            Folder.Visibility = Visibility.Hidden;
            Plus.Visibility = Visibility.Hidden;
            anotherImage.Visibility = Visibility.Hidden;
            canvas.Visibility = Visibility.Hidden;
            for (int i = 0; i < edgesPolygon.Count; i++)
            {
                Point p = new Point((edgesPolygon.ElementAt(i).x), edgesPolygon.ElementAt(i).y);
                polygon.Points.Add(p);
            }
            polygon.StrokeThickness = 2;
            polygon.Stroke = Brushes.Blue;
            canvasPolygon.Children.Add(polygon);
            SubmitPolygonMessage.Visibility = Visibility.Visible;
            FinalSubmit.Visibility = Visibility.Visible;
            ChoosePolygonAgain.Visibility = Visibility.Visible;
        }

        private void ChoosePolygonAgain_Click(object sender, RoutedEventArgs e)
        {
            InstructionText.Visibility = Visibility.Visible;
            Load.Visibility = Visibility.Visible;
            UploadSymbol.Visibility = Visibility.Visible;
            Folder.Visibility = Visibility.Visible;
            Plus.Visibility = Visibility.Visible;
            anotherImage.Visibility = Visibility.Visible;
            canvas.Visibility = Visibility.Visible;

            canvasPolygon.Children.Remove(polygon);
            SubmitPolygonMessage.Visibility = Visibility.Hidden;
            FinalSubmit.Visibility = Visibility.Hidden;
            ChoosePolygonAgain.Visibility = Visibility.Hidden;
        }
        private void PolygonSubmitted(object sender, RoutedEventArgs e)
        {
            string json =  System.Text.Json.JsonSerializer.Serialize(polygon.Points);

            string startupPath = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo newPath = System.IO.Directory.GetParent(startupPath);
            newPath = System.IO.Directory.GetParent(newPath.FullName);
            newPath = System.IO.Directory.GetParent(newPath.FullName);
            newPath = System.IO.Directory.GetParent(newPath.FullName);

            string fileName = newPath.FullName + "\\PolygonData.json";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(json);
            }
            DirectoryInfo di = new DirectoryInfo(newPath + "\\Images\\image");
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }

            System.IO.File.Copy(((BitmapImage)imgPhoto.Source).UriSource.AbsolutePath,
                newPath + "\\Images\\image\\img.jpg");

        }
       
    }
}
