namespace UsersMS.Infrastructure.Adapters.Keycloak.RequestBuilder.DTO
{
    public class KeycloakRequestDTO
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }

        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? GrantType { get; set; }

        public bool? EmailVerified { get; set; } = false;
        public bool? Enabled { get; set; } = true;

        // Estructura para definir credenciales (ej: {"type": "password", "value": "123", "temporary": false})
        public List<Dictionary<string, object>> Credentials { get; set; } = new();
    }
}
