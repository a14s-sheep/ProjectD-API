using MailKit.Security;
using MimeKit;
using System.Net.Mail;

namespace ProjectD_API.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_settings.SenderEmail));
            email.To.Add(new MailboxAddress("", toEmail));

            email.Subject = subject;
            email.Body = new TextPart("html") { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(
                _settings.Server,
                _settings.Port,
                _settings.UseSSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls
            );
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
