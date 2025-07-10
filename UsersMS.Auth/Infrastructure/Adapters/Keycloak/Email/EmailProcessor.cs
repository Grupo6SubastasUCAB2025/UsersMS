using UsersMS.Core.Application;
using UsersMS.Infrastructure.DTOs.Email;

namespace UsersMS.Infrastructure.Adapters.Keycloak.Email
{
    public class EmailProcessor
    {
        private readonly EmailTemplateService _emailTemplateService;
        private readonly IService<EmailRequestDTO, EmailResponseDTO> _emailService;

        public EmailProcessor(EmailTemplateService emailTemplateService, IService<EmailRequestDTO, EmailResponseDTO> emailService)
        {
            _emailTemplateService = emailTemplateService;
            _emailService = emailService;
        }

        public async Task<EmailResponseDTO> SendEmailAsync(string toEmail, string subject, string templateName, Dictionary<string, string> placeholders)
        {
            try
            {
                var emailBody = await _emailTemplateService.LoadTemplateAsync(templateName);
                emailBody = _emailTemplateService.ReplacePlaceholders(emailBody, placeholders);

                var emailRequest = new EmailRequestDTO
                {
                    ToEmail = toEmail,
                    Subject = subject,
                    Body = emailBody
                };

                return await _emailService.Execute(emailRequest);
            }
            catch (FileNotFoundException ex)
            {
                return new EmailResponseDTO { Success = false, Message = $"Template not found: {ex.Message}", Time = DateTime.UtcNow };
            }
        }
    }
}
