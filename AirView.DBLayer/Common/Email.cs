
using System.Net.Mail;

namespace SWI.Libraries.Common
{
    public class Email
    {
        private string Smtp { get; set; }
        private int Port { get; set; }
        private string FromEmail { get; set; }
        private string FromEmailPassword { get; set; }

        public Email() {

        }
        public Email(string smtp,int port,string fromEmail,string fromEmailPassword)
        {
            Smtp = smtp;
            Port = port;
            FromEmail = fromEmail;
            FromEmailPassword = fromEmailPassword;
        }
        public void SendGmail(string subject, string htmlBody, string recipients)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("muzzammil.saim@gmail.com");
            mail.To.Add(recipients);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = htmlBody;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("muzzammil.saim@gmail.com", "  ");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        public void SendEmail(string subject, string htmlBody, string recipients)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Smtp);
                mail.From = new MailAddress(FromEmail);
                mail.To.Add(recipients);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = htmlBody;
                SmtpServer.Port = Port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(FromEmail, FromEmailPassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);


               

            }
            catch (System.Exception)
            {

                throw;
            }
            
        }


    }
}