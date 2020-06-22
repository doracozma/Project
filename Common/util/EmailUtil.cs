using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EmailUtil
    {

        private static readonly string SMTP_SERVER = "mail.vssoft.ro";
        private static readonly string SMTP_USERNAME = "weddinger@vssoft.ro";
        private static readonly string SMTP_PASSWORD = "t0742475721.";


        public static async void sendEmail(string email, string content, string subject)
        {
            SmtpClient client = new SmtpClient(SMTP_SERVER);
            client.EnableSsl = false;
            client.Port = 8889;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = SMTP_SERVER;
            client.Credentials = new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(SMTP_USERNAME);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = content;
            mailMessage.IsBodyHtml = true;

            using (client)
            {
                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "");
                }
            }
        }
    }
}
