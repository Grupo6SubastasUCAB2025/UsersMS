using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;
using UsersMS.Commons.Exceptions;
using UsersMS.Infrastructure.Adapters.Keycloak.RequestBuilder.DTO;

namespace UsersMS.Infrastructure.Adapters.Keycloak.RequestBuilder
{
    public class KeycloakRequestBuilder : IKeycloakRequestBuilder
    {
        private readonly IConfiguration _configuration;
        private KeycloakRequestDTO _request;
        private Dictionary<string, object>? _newPassword;

        public KeycloakRequestBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
            _request = new KeycloakRequestDTO();
        }

        public IKeycloakRequestBuilder WithToken(string token)
        {
            _request.Token = token ?? throw new ArgumentException("Token no puede ser null.");
            return this;
        }

        public IKeycloakRequestBuilder WithRefreshToken(string refreshToken)
        {
            _request.RefreshToken = refreshToken ?? throw new ArgumentException("Refresh token no puede ser null.");
            return this;
        }

        public IKeycloakRequestBuilder WithClientId()
        {
            var clientId = _configuration["Keycloak:ClientId"];
            if (string.IsNullOrEmpty(clientId))
                throw new InvalidOperationException("ClientId esta faltante en la configuracion.");

            _request.ClientId = clientId;
            return this;
        }

        public IKeycloakRequestBuilder WithClientSecret()
        {
            var secret = _configuration["Keycloak:ClientSecret"];
            if (string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("ClientSecret esta faltante en la configuracion.");

            _request.ClientSecret = secret;
            return this;
        }

        public IKeycloakRequestBuilder WithCredentials(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ConfigurationException("Password cannot be null or empty.");
            }
            var credentials = new Dictionary<string, object>
               {
                    { "type", "password" },
                    { "value", password }
               };
            _request.Credentials.Add(credentials);
            return this;
        }

        public IKeycloakRequestBuilder WithNewPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ConfigurationException("Password cannot be null or empty.");
            }
            _newPassword = new Dictionary<string, object>
               {
               { "type", "password" },
                    { "value", password }
               };
            return this;
        }

        public object GetNewPasswordData()
        {
            return new
            {
                type = _newPassword?["type"],
                value = _newPassword?["value"],
            };
        }

        public object GetRoleData(string roleId, string roleName)
        {
            return new[]
            {
                    new { id = roleId, name = roleName }
               };
        }

        public IKeycloakRequestBuilder WithUsername(string username)
        {
            _request.Username = username ?? throw new ArgumentException("Username no puede ser null.");
            return this;
        }

        public IKeycloakRequestBuilder WithEmail(string email)
        {
            _request.Email = email ?? throw new ArgumentException("Email no puede ser null.");
            return this;
        }

        public IKeycloakRequestBuilder WithPassword(string password)
        {
            _request.Password = password ?? throw new ArgumentException("Password no puede ser null.");
            return this;
        }

        public IKeycloakRequestBuilder WithGrantType(string grantType)
        {
            _request.GrantType = grantType ?? throw new ArgumentException("Grant type no puede ser null.");
            return this;
        }

        public IKeycloakRequestBuilder WithEmailVerified(bool emailVerified = true)
        {
            _request.EmailVerified = emailVerified;
            return this;
        }

        public IKeycloakRequestBuilder WithEnabled(bool enabled = true)
        {
            _request.Enabled = enabled;
            return this;
        }

        public object GetUserData()
        {
            return new
            {
                username = _request.Username,
                email = _request.Email,
                enabled = _request.Enabled,
                emailVerified = _request.EmailVerified,
                credentials = _request.Credentials
            };
        }

        public IEnumerable<KeyValuePair<string, string>> BuildAsForm()
        {
            var dict = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(_request.Token)) dict["token"] = _request.Token;
            if (!string.IsNullOrEmpty(_request.RefreshToken)) dict["refresh_token"] = _request.RefreshToken;
            if (!string.IsNullOrEmpty(_request.ClientId)) dict["client_id"] = _request.ClientId;
            if (!string.IsNullOrEmpty(_request.ClientSecret)) dict["client_secret"] = _request.ClientSecret;
            if (!string.IsNullOrEmpty(_request.Username)) dict["username"] = _request.Username;
            if (!string.IsNullOrEmpty(_request.Password)) dict["password"] = _request.Password;
            if (!string.IsNullOrEmpty(_request.GrantType)) dict["grant_type"] = _request.GrantType;

            return dict;
        }

        public StringContent BuildJson(object data)
        {
            var json = JsonSerializer.Serialize(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
