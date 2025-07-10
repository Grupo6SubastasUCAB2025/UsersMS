using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using UsersMS.Infrastructure.DTOs.Email;

namespace UsersMS.Infrastructure.Adapters.Keycloak.Email
{
    public class SmtpClientFactory
    {
        private readonly IConfiguration _configuration;

        public SmtpClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string SmtpServer, int SmtpPort, string SmtpUsername, string SmtpPassword, string FromEmail, string FromDisplayName) GetEmailSettings()
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPortString = _configuration["EmailSettings:SmtpPort"];
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var fromDisplayName = _configuration["EmailSettings:FromDisplayName"];

            if (string.IsNullOrEmpty(smtpServer) ||
                string.IsNullOrEmpty(smtpPortString) ||
                string.IsNullOrEmpty(smtpUsername) ||
                string.IsNullOrEmpty(smtpPassword) ||
                string.IsNullOrEmpty(fromEmail) ||
                string.IsNullOrEmpty(fromDisplayName))
            {
                throw new InvalidOperationException("Missing email configuration");
            }

            if (!int.TryParse(smtpPortString, out var smtpPort))
            {
                throw new InvalidOperationException("Invalid SMTP port configuration");
            }

            return (smtpServer, smtpPort, smtpUsername, smtpPassword, fromEmail, fromDisplayName);
        }

        public void ValidateEmailSettings((string SmtpServer, int SmtpPort, string SmtpUsername, string SmtpPassword, string FromEmail, string FromDisplayName) emailSettings)
        {
            if (string.IsNullOrEmpty(emailSettings.SmtpServer) ||
                string.IsNullOrEmpty(emailSettings.SmtpUsername) ||
                string.IsNullOrEmpty(emailSettings.SmtpPassword) ||
                string.IsNullOrEmpty(emailSettings.FromEmail) ||
                string.IsNullOrEmpty(emailSettings.FromDisplayName))
            {
                throw new InvalidOperationException("Missing email configuration");
            }
        }

        public SmtpClient CreateSmtpClient((string SmtpServer, int SmtpPort, string SmtpUsername, string SmtpPassword, string FromEmail, string FromDisplayName) emailSettings)
        {
            var smtpClient = new SmtpClient(emailSettings.SmtpServer)
            {
                Port = emailSettings.SmtpPort,
                Credentials = new NetworkCredential(emailSettings.SmtpUsername, emailSettings.SmtpPassword),
                EnableSsl = true
            };

            return smtpClient;
        }

        public MailMessage CreateMailMessage(EmailRequestDTO emailRequest, (string SmtpServer, int SmtpPort, string SmtpUsername, string SmtpPassword, string FromEmail, string FromDisplayName) emailSettings)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings.FromEmail, emailSettings.FromDisplayName),
                Subject = emailRequest.Subject,
                Body = emailRequest.Body,
                IsBodyHtml = false,
            };
            mailMessage.To.Add(emailRequest.ToEmail);

            return mailMessage;
        }
    }
}
