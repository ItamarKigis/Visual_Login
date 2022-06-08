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

namespace Navigation_Drawer_App
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        Menu obj = new Menu();
        public Settings()
        {
            InitializeComponent();
            input.Text = (Menu.pointAccuracy).ToString();
            widthScroll.Value = Menu.width;
            heightScroll.Value = Menu.height;
            input.TextChanged += new TextChangedEventHandler(pointAccuracyChanged);
        }

        private void WidthChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Menu.width = widthScroll.Value;
        }
        private void HeightChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Menu.height = heightScroll.Value;
        }
        private void pointAccuracyChanged(object Sender, TextChangedEventArgs e)
        {
            Menu.pointAccuracy = int.Parse(input.Text);
        }
    }
}
