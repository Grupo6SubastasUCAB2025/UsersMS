namespace UsersMS.Infrastructure.Adapters.Keycloak.Email
{
    public class EmailTemplateService
    {
        private readonly string _templatesPath;

        public EmailTemplateService()
        {
            _templatesPath = Path.Combine(Directory.GetCurrentDirectory(), "../Auth/Templates");//Falta implementar
        }

        public async Task<string> LoadTemplateAsync(string templateName)
        {
            var templatePath = Path.Combine(_templatesPath, templateName);
            return await File.ReadAllTextAsync(templatePath);
        }

        public string ReplacePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            foreach (var placeholder in placeholders)
            {
                template = template.Replace($"{{{{{placeholder.Key}}}}}", placeholder.Value);
            }
            return template;
        }
    }
}
