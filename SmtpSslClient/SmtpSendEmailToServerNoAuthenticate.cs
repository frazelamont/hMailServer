using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SmtpClientSSL
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SmtpClient smtp = new SmtpClient("aspmx.l.google.com");
                smtp.EnableSsl = true;
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                // smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                // smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                // smtp.Credentials = new NetworkCredential(textBox1.Text, "password");
                MailMessage msg = new MailMessage("hello@fxstar.eu", "ggggg@gmail.com", "Smtp no auth test", "Hello from c# client no authentication");
                smtp.Send(msg);
                Console.WriteLine("Email was send");
                Console.ReadKey();
            }catch(Exception c){
                Console.WriteLine( c.ToString() );
                Console.ReadKey();
            }
        }
    }
}
