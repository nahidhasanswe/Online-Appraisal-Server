using System;
using System.Net.Mail;
using System.Threading;
using System.Web;

namespace AppraisalSystem.Services
{
    public class EmailNotifier
    {
        private string ServerConts = System.Configuration.ConfigurationSettings.AppSettings["ClientPath"].ToString();

        public void Send(string subject, string message, string receiver)
        {
            string body = "";
            
            body = HttpUtility.HtmlEncode(message);
                    MailMessage mail = new MailMessage(System.Configuration.ConfigurationSettings.AppSettings["FromAddress"],receiver);
                    SmtpClient client = new SmtpClient();
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationSettings.AppSettings["FromAddress"], System.Configuration.ConfigurationSettings.AppSettings["Password"]);
                    client.EnableSsl = true;
                    client.Port = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["SMTPPort"]);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.DeliveryFormat = SmtpDeliveryFormat.International;
                    
                    client.Host = System.Configuration.ConfigurationSettings.AppSettings["SmtpClient"];
                    client.Credentials = credentials;
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    client.Send(mail);
        }
    }
}