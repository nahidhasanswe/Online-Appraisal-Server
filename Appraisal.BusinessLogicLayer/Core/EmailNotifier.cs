using System;
using System.Net.Mail;
using System.Threading;

namespace Appraisal.BusinessLogicLayer.Core
{
    public class EmailNotifier
    {
        private string ServerConts = System.Configuration.ConfigurationSettings.AppSettings["ClientPath"].ToString();

        public void Send(string url, string message, string receiver, string sender)
        {
            string body = "";
            if (url != "")
            {
                body = message + "<br/> Please click the following <a href=" + ServerConts + url +
                               "> link </a> to view details.<br/><br/>Best Regards,<br/> " + sender;
            }
            else
            {
                body = message + "<br/><br/>Best Regards,<br/> " + sender;
            }

            MailMessage mail = new MailMessage(System.Configuration.ConfigurationSettings.AppSettings["FromAddress"], receiver);
            SmtpClient client = new SmtpClient();
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationSettings.AppSettings["FromAddress"], System.Configuration.ConfigurationSettings.AppSettings["Password"]);
            client.EnableSsl = true;
            client.Port = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["SMTPPort"]);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.DeliveryFormat = SmtpDeliveryFormat.International;

            client.Host = System.Configuration.ConfigurationSettings.AppSettings["SmtpClient"];
            client.Credentials = credentials;
            mail.Subject = "Notification Email";
            mail.Body = body;
            mail.IsBodyHtml = true;
            client.Send(mail);
        }
    }
}