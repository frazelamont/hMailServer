using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SmtpCLientSSL
{
    class Program
    {

        // "Server SSL Certyficate (CN=www.domain.com)"
        public static string hostname = "fxstar.eu";

        // "Server host www.example.com"
        public static string host = "localhost";

        // "Server port"
        public static int port = 587;

        List<string> PosOpenID = new List<string>();

        public static string txt = "";

        private static Hashtable certificateErrors = new Hashtable();

        static void Main(string[] args)
        {
            MailMessage message = new MailMessage("hello@fxstar.eu","hello@breakermind.com","SMTP Test", "Hello from c# smtp client with sockets.");
            ConnectSSL(message);
            Console.ReadKey();
        }

        public static void ConnectSSL(MailMessage message)
        {

            try
            {
                TcpClient client = new TcpClient(host, port);                
                SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                try
                {
                    sslStream.AuthenticateAsClient(hostname);
                }
                catch (Exception e)
                {
                    Print(e.ToString());
                    client.Close();
                    return;
                }

                // Send Hello
                Senddata(sslStream, string.Format("HELO {0}\r\n", Dns.GetHostName()));
                if (Response(sslStream) != 220)
                {
                    sslStream.Close();
                    return;
                }

                // Send from 
                Senddata(sslStream, string.Format("MAIL From: {0}\r\n", message.From));
                if (Response(sslStream) != 250)
                {
                    sslStream.Close();
                    return;
                }
                

                // Send to
                Senddata(sslStream, string.Format("RCPT TO: {0}\r\n", message.To));
                if (Response(sslStream) != 250)
                {
                    sslStream.Close();
                    return;
                }

                /*
                string _To = message.To.ToString();
                string[] Tos = _To.Split(new char[] { ',' });
                foreach (string To in Tos)
                {
                    Senddata(sslStream, string.Format("RCPT TO: {0}\r\n", To));
                    Console.WriteLine("Response To " + Response(sslStream));
                    if (Response(sslStream) != 250)
                    {
                        sslStream.Close();
                        return;
                    }
                }

                if (message.CC != null)
                {
                    string bbb = message.CC.ToString();
                    Tos = bbb.Split(new char[] { ',' });
                    foreach (string To in Tos)
                    {
                        Senddata(sslStream, string.Format("RCPT TO: {0}\r\n", To));
                        Console.WriteLine("Response CC " + Response(sslStream));
                        if (Response(sslStream) != 250)
                        {
                            sslStream.Close();
                            return;
                        }
                    }
                }
                */

                StringBuilder Header = new StringBuilder();
                Header.Append("Subject: " + message.Subject + "\r\n");
                Header.Append("From: " + message.From + "\r\n");
                string mmm = message.To.ToString();                
                string[] Tos = mmm.Split(new char[] { ';' });
                Header.Append("To: ");
                for (int i = 0; i < Tos.Length; i++)
                {
                    Header.Append(i > 0 ? ";" : "");
                    Header.Append(Tos[i]);
                }
                Header.Append("\r\n");
                if (message.CC != null)
                {
                    string nnn = message.CC.ToString();
                    Tos = nnn.Split(new char[] { ';' });
                    Header.Append("Cc: ");
                    for (int i = 0; i < Tos.Length; i++)
                    {
                        Header.Append(i > 0 ? ";" : "");
                        Header.Append(Tos[i]);
                    }
                    Header.Append("\r\n");
                }
                Header.Append("Date: ");
                Header.Append(DateTime.Now.ToString("ddd, d M y H:m:s z"));
                Header.Append("\r\n");
                Header.Append("Subject: " + message.Subject + "\r\n");
                Header.Append("X-Mailer: SMTP c# \r\n");
                //Header.Append("Content-Type: text/html\r\n");
                string MsgBody = message.Body;
                if (!MsgBody.EndsWith("\r\n"))
                    MsgBody += "\r\n";

                
                Senddata(sslStream, "DATA\r\n");                
                if (Response(sslStream) != 250)
                {
                    sslStream.Close();
                    return;
                }

                Header.Append("\r\n");
                Header.Append(MsgBody);
                Header.Append("\r\n");
                Header.Append(".\r\n");
                               
                string message1 = "Subject: " + message.Subject + "\r\n";
                message1 += "To: " + message.To + "\r\n"; 
                message1 += "From: " + message.From + "\r\n";
                string bodyHtml = message.Body;
                if (bodyHtml.Length > 0)
                {
                    message1 += "MIME-Version: 1.0\r\n"
                        + " Content-Type: text/ html;\r\n"
                        + " charset=\" utf-8\"\r\n";
                    message1 += "\r\n" + bodyHtml;
                }
                else
                {
                    message1 += "\r\n" + bodyHtml;
                };
                message1 += "\r\n.\r\n"; 

                Senddata(sslStream, Header.ToString());
                // Senddata(sslStream, message1);                
                if (Response(sslStream) != 250)
                {
                    sslStream.Close();
                    return;
                }
                
                Senddata(sslStream, "QUIT\r\n");
                Response(sslStream);
                sslStream.Close();                

                // Close the client connection.
                client.Close();
                Print("Client closed.");


            }
            catch (ArgumentNullException e)
            {
                Print("ArgumentNullException: " + e);
            }
            catch (SocketException e)
            {
                Print("SocketException: " + e);
            }
        }

        public static void Senddata(SslStream sslStream, string msg){
                byte[] messsage = Encoding.UTF8.GetBytes(msg);
                sslStream.Write(messsage);
                sslStream.Flush();
        }

        public static Int32 Response(SslStream sslStream)
        {
            int response;
            string serverMessage = ReadMessage(sslStream);
            Print("Server says: " + serverMessage);
            return response = Convert.ToInt32(serverMessage.Substring(0, 3));            
        }

        public static void Print(string txt) {
            Console.WriteLine(txt);
        }

        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            //return false;
            //Force ssl certyfikates as correct
            return false;
        }

        static string ReadMessage(SslStream sslStream)
        {
            // Read the  message sent by the server. 
            // The end of the message is signaled using the 
            // "<EOF>" marker.
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                // Use Decoder class to convert from bytes to UTF8 
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                // Check for EOF. 
                if (messageData.ToString().IndexOf("") != -1)
                {
                    break;
                }
            } while (bytes != 0);

            return messageData.ToString();
        }

    }
}
