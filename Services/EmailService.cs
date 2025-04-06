using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SocialAssistanceFundMisMcv.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
    public class EmailService : IEmailService
    {
        private readonly string smtpServer = "smtp-relay.brevo.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUsername = "89b417002@smtp-brevo.com";
        private readonly string smtpPassword = "QtIKX107kpHBS62N";

        private readonly string fromEmail = "89b417001@smtp-brevo.com";
        private readonly string fromName = "SOCIAL ASSISTANCE FUND";

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(fromName, fromEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpUsername, smtpPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
