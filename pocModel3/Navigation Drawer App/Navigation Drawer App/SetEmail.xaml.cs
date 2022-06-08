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
using System.IO;
namespace Navigation_Drawer_App
{
    /// <summary>
    /// Interaction logic for SetEmail.xaml
    /// </summary>
    public partial class SetEmail : UserControl
    {
        public SetEmail()
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\משתמש\Documents\pocModel3\Email.txt");
            InitializeComponent();
            if(text == "")
            {
                SetEmailMsg.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeEmailMsg.Visibility = Visibility.Visible;
            }
            inputEmail.Text = text;
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(@"C:\Users\משתמש\Documents\pocModel3\Email.txt", inputEmail.Text);
        }
    }
}
