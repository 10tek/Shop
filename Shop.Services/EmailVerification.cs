using Shop.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Shop.Services
{
    public class EmailVerification : IRegistration
    {
        private MailMessage mail = new MailMessage();
        private SmtpClient SmtpServer = new SmtpClient("smtp.mail.ru");
        private static Random random = new Random();

        public int SendCode(string email)
        {
            var verificationCode = random.Next(1000, 10000);

            mail.From = new MailAddress("10tek3@mail.ru");
            mail.To.Add(email);
            mail.Subject = "Verification Code";
            mail.Body = $"Your verification code: {verificationCode}";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("10tek3@mail.ru", "mete0rblast123");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            return verificationCode;
        }
    }
}
