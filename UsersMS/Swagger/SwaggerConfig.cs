namespace UsersMS.Swagger
{
    public class SwaggerConfig
    {
        public string Title { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string RoutePrefix { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AuthorizationConfig Authorization { get; set; } = new AuthorizationConfig();
    }

    public class AuthorizationConfig
    {
        public string Name { get; set; } = string.Empty;
        public string In { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Scheme { get; set; } = string.Empty;
    }
}
