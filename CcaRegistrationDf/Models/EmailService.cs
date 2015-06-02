using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace CcaRegistrationDf.Models
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(message.Destination.ToString());
                mail.From = new MailAddress("noreply@noreply.schools.utah.gov");
                mail.Subject = message.Subject;
                mail.Body = message.Body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "198.60.12.9";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = true;

                await smtp.SendMailAsync(mail);
            }
            catch
            {
                throw;
            }
        }
    }
}