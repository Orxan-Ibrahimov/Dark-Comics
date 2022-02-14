using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DarkComics.Helpers.Methods
{
    public static class MailOpertions
    {
        public static void SendMessage(string mail,string subject,string message,bool isHtml = false)
        {

            var client = new SmtpClient();

            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("orkhanaib@code.edu.az", "Orxan620");

            var mailMessage = new System.Net.Mail.MailMessage("orkhanaib@code.edu.az", mail);

            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.Priority = MailPriority.High;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            mailMessage.IsBodyHtml = isHtml;

            client.Send(mailMessage);

            client.Dispose();
            
        }

    }
}
