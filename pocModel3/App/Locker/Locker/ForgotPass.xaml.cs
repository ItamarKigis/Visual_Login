using System.Windows.Controls;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.IO;
namespace Locker
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ForgotPass : UserControl
    {
        private static Random random = new Random();
        string code = RandomString(8);
        public ForgotPass()
        {
            InitializeComponent();
            sendEmail();
        }

        private void SubmitCode_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(Input.Text == code)
            {
                Environment.Exit(0);
            }
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void sendEmail()
        {
            string readText = File.ReadAllText(@"C:\Users\משתמש\Documents\pocModel3\Email.txt");
            MailMessage msg = new MailMessage(readText, readText, "Visual Login Authentication code",
                "Your code for passing the authentication is: " + code);
            msg.IsBodyHtml = true;

            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.UseDefaultCredentials = false;
            NetworkCredential cre = new NetworkCredential("itamarkigis@haderahigh.org.il", "kigis2004");//your mail password
            sc.Credentials = cre;
            sc.EnableSsl = true;

            sc.Send(msg);
        }
    }
}
