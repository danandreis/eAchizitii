using eAchizitii.Data.ViewModels;
using System.Net.Mail;
using System.Net;
using eAchizitii.Models;
using Microsoft.AspNetCore.Identity;

namespace eAchizitii.Data.Services
{
    public class EmailService : IEmailService
    {
       
        public bool TrimiteMail(string to, string from, string subject, string body)
        {

            MailAddress destinationAddress = new MailAddress(to);
            MailAddress senderAddress = new MailAddress(from);
            MailMessage mesaj = new MailMessage(senderAddress, destinationAddress);

            mesaj.IsBodyHtml = true;
            mesaj.Subject = subject;
            mesaj.Body = body;

            SmtpClient smtpClient = new SmtpClient("smtp.mailtrap.io", 2525);
            smtpClient.Credentials = new NetworkCredential("d3e979d43217c6", "a49a97b23d8a21");
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mesaj);
                return true;

            }
            catch (Exception ex)
            {

                return false;

            }

        }


    }
}
