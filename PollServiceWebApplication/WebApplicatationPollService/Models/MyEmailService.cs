using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace WebApplicatationPollService.Models {
    public class MyEmailService : IIdentityMessageService {
        public async Task SendAsync(IdentityMessage message) {
            var mailMessage = new MailMessage() { Subject = message.Subject, Body = message.Body, IsBodyHtml = true, From = new MailAddress("####") };
            mailMessage.To.Add(new MailAddress(message.Destination));
            using (var smtp = new SmtpClient()) {
                smtp.Port = 587;
                smtp.Host = "####";
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential() {
                    UserName = "####", Password = "#####"
                };
                await smtp.SendMailAsync(mailMessage);
            }
        }
    }
}