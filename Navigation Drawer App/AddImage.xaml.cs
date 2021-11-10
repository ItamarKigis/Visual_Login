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
using Bitmap = System.Drawing.Bitmap;

namespace Navigation_Drawer_App
{
    /// <summary>
    /// Interaction logic for AddImage.xaml
    /// </summary>
    public partial class AddImage : UserControl
    {
        int[] x = new int[20];
        int[] y = new int[20];
        int[] w = new int[20];
        int[] h = new int[20];
        int originalHeight = 0;
        int originalWidth = 0;
        int NumOfObjects = 0;
        string ImgPath;
        public AddImage()
        {
            InitializeComponent();
            this.DataContext = this;

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
            originalWidth =Int32.Parse(orignalShape.Split(' ')[0]);
            originalHeight = Int32.Parse(orignalShape.Split(' ')[1]);
        }
        private void drawRectangles()
        {
            double imageHeightRatio = 0;
            double imageWidthRatio = 0;
            imageHeightRatio = 465.0 / originalHeight;
            imageWidthRatio = 670.0 / originalWidth;

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
                Canvas.SetTop(rect, y[i]* imageHeightRatio); // y of rectangle
                Canvas.SetLeft(rect, x[i] * imageWidthRatio); // x of rectangle
                
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
                        /*runEdgeDetection(@"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python36_64\python.exe",
                    "C:\\Users\\משתמש\\Documents\\pocModel3\\edgeDetection.py " + ImgPath + " " + X + " " + Y + 
                    " " +  W + " " + H);*/
                        UserLoginSequence win2 = new UserLoginSequence();
                        win2.Show();
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
                    Console.WriteLine(result);
                }
            }
        }
    }
}
