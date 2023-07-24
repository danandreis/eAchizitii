using System.Net;
using System.Net.Mail;

namespace eAchizitii.Email
{
    public class SendEmail
    {

        public string Destinatar { get; set; }
        public string Mesaj { get; set; }
        public string Subiect { get; set; }

        public SendEmail(string destinatar, string mesaj, string subiect)
        {
            Destinatar = destinatar;
            Mesaj = mesaj;
            Subiect = subiect;
        }

        public Task Send()
        {

            MailAddress destinatar = new MailAddress(Destinatar);
            
            MailAddress from = new MailAddress("admin@achizitii.com");
            
            MailMessage mesaj = new MailMessage(from, destinatar);
            mesaj.Subject = Subiect;
            mesaj.IsBodyHtml = true;
            mesaj.Body = Mesaj;

            SmtpClient smtp = new SmtpClient("mail.HostDan.ro", 25)
            {

                Credentials = new NetworkCredential("*****", "*****"),
                EnableSsl = true

            };

            smtp.UseDefaultCredentials = true;

            try
            {
                smtp.Send(mesaj);
                return Task.CompletedTask;

            }catch (SmtpException e)
            {

                return Task.FromException(e);
                 
            }
        }
    }
}
