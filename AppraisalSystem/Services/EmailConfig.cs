using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using SendGrid.Helpers.Mail;

namespace AppraisalSystem.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            MailMessage email = new MailMessage(new MailAddress("globalonlinebd.net@gmail.com", "(Do not reply)"),
                new MailAddress(message.Destination));
            email.Subject = message.Subject;
            email.Body = message.Body;
            email.IsBodyHtml = true;
            using (var mailClient = new EmailGateway())
            {
                //email.From = new MailAddress(mailClient.UserName, "don't reply");
                await mailClient.SendMailAsync(email);
            }
        }
    }
    public class EmailGateway : SmtpClient
    {
        public string UserName { set; get; }

        public EmailGateway()
            : base(
                ConfigurationManager.AppSettings["SmtpClient"],
                Int32.Parse(ConfigurationManager.AppSettings["SMTPPort"]))
        {
            this.UserName = ConfigurationManager.AppSettings["UserID"];
            this.EnableSsl = true;
            this.UseDefaultCredentials = false;
            this.Credentials = new System.Net.NetworkCredential(this.UserName,
                ConfigurationManager.AppSettings["Password"]);
        }
    }
}