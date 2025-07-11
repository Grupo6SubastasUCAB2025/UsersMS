using Microsoft.Extensions.Configuration;
using System.Text.Json;
using UsersMS.Commons.Exceptions;
using UsersMS.Infrastructure.Adapters.Keycloak.RequestBuilder;
using UsersMS.Infrastructure.Adapters.Keycloak.UrlHelper;
using UsersMS.Infrastructure.Adapters.Keycloak;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Graph.Models;

namespace UsersMS.Infrastructure.Adapters.KeycloakRepository
{
    public class KeycloakRepository : IKeycloakRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IUrlHelperKeycloak _urlHelperKeycloak;
        private readonly IKeycloakRequestBuilder _keycloakRequestBuilder;

        public KeycloakRepository(IConfiguration configuration, IUrlHelperKeycloak urlHelperKeycloak, IKeycloakRequestBuilder keycloakRequestBuilder)
        {
            _configuration = configuration;
            _urlHelperKeycloak = urlHelperKeycloak;
            _keycloakRequestBuilder = keycloakRequestBuilder;
        }

        public async Task<(string AccessToken, string RefreshToken)> GetTokenAsync(HttpClient client, string email, string password)
        {
            var tokenEndpoint = _urlHelperKeycloak.GetTokenUrl(_configuration);

            var tokenRequest = _keycloakRequestBuilder
                .WithClientId()
                .WithClientSecret()
                .WithUsername(email)
                .WithPassword(password)
                .WithGrantType("password")
                .BuildAsForm();

            var response = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(tokenRequest));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = JsonDocument.Parse(content).RootElement.GetProperty("error_description").GetString();
                var error = JsonDocument.Parse(content).RootElement.GetProperty("error").GetString();

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest && error == "invalid_grant")
                {
                    throw new UnauthorizedException("Account setup incomplete or credentials invalid.", new List<string> { errorDetails ?? "Unknown error." });
                }

                throw new UnauthorizedException("Invalid credentials.", new List<string> { errorDetails ?? "Unknown error." });
            }

            var root = JsonDocument.Parse(content).RootElement;

            return (
                root.GetProperty("access_token").GetString() ?? throw new UnauthorizedAccessException("Missing access token."),
                root.GetProperty("refresh_token").GetString() ?? throw new UnauthorizedAccessException("Missing refresh token.")
            );
        }

        public async Task<string> GetClientCredentialsTokenAsync(HttpClient client)
        {
            var tokenEndpoint = _urlHelperKeycloak.GetTokenUrl(_configuration);
            var tokenRequest = _keycloakRequestBuilder
                .WithClientId()
                .WithClientSecret()
                .WithGrantType("client_credentials")
                .BuildAsForm();

            var response = await client.PostAsync(tokenEndpoint, new FormUrlEncodedContent(tokenRequest));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonDocument.Parse(content).RootElement.GetProperty("error_description").GetString();
                throw new UnauthorizedException("Could not get client_credentials token", new List<string> { error ?? "Unknown error" });
            }

            return JsonDocument.Parse(content).RootElement.GetProperty("access_token").GetString() ?? "";
        }

        public async Task<(string UserId, string Role, string Email)> IntrospectTokenAsync(HttpClient client, string token)
        {
            var endpoint = _urlHelperKeycloak.GetIntrospectUrl(_configuration);
            var form = new FormUrlEncodedContent(
                _keycloakRequestBuilder.WithToken(token).WithClientId().WithClientSecret().BuildAsForm()
            );

            var response = await client.PostAsync(endpoint, form);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Token introspection failed.");

            var root = JsonDocument.Parse(content).RootElement;
            if (!root.GetProperty("active").GetBoolean())
                throw new UnauthorizedAccessException("Inactive token.");

            var userId = root.GetProperty("sub").GetString() ?? throw new UnauthorizedAccessException("Missing userId.");
            var email = root.GetProperty("email").GetString() ?? throw new UnauthorizedAccessException("Missing email.");
            var clientId = _configuration["Keycloak:ClientId"] ?? throw new ConfigurationException("ClientId missing.");

            var role = root.GetProperty("resource_access").GetProperty(clientId).GetProperty("roles")[0].GetString();
            var allowed = _configuration.GetSection("Roles").Get<List<string>>() ?? new();

            if (!allowed.Contains(role))
                throw new UnauthorizedAccessException($"Role {role} is not allowed.");

            return (userId, role!, email);
        }

        public async Task<bool> CreateUserAsync(HttpClient client, string email, string password, Dictionary<string, string> attributes)
        {
            var endpoint = _urlHelperKeycloak.GetUserEndpoint(_configuration);

            var userData = new
            {
                email = email,
                enabled = true,
                emailVerified = true,
                credentials = new[]
                {
                    new { type = "password", value = password, temporary = false }
                },
                attributes = attributes
            };

            var content = _keycloakRequestBuilder.BuildJson(userData);
            var response = await client.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException($"User creation failed: {responseContent}");
            }

            return true;
        }




        public async Task<(string UserId, bool HasRequiredAction)> GetUserByEmailAsync(HttpClient client, string email, string requiredAction)
        {
            var endpoint = _urlHelperKeycloak.GetUserByEmailUrl(_configuration, email);
            var response = await client.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException($"Get user by email failed: {content}");

            var root = JsonDocument.Parse(content).RootElement;
            if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
                throw new UnauthorizedAccessException("User not found.");

            var user = root[0];
            var id = user.GetProperty("id").GetString() ?? throw new UnauthorizedAccessException("Missing userId.");

            var hasRequiredAction = user.TryGetProperty("requiredActions", out var actions) && actions.EnumerateArray().Any(a => a.GetString() == requiredAction);

            return (id, hasRequiredAction);
        }

        public async Task<string> GetClientIdAsync(HttpClient cliente)
        {
            var endpoint = _urlHelperKeycloak.GetClientsEndpoint(_configuration);
            var response = await cliente.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Get client ID failed: {content}");

            var root = JsonDocument.Parse(content).RootElement;
            var client = root.EnumerateArray().FirstOrDefault(c => c.GetProperty("clientId").GetString() == _configuration["Keycloak:ClientId"]);
            return client.GetProperty("id").GetString() ?? throw new Exception("Client ID missing.");
        }

        public async Task<string> GetRoleAsync(HttpClient client, string clientId, string roleName)
        {
            var endpoint = _urlHelperKeycloak.GetClientRolesEndpoint(_configuration, clientId);
            var response = await client.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Get roles failed: {content}");

            var root = JsonDocument.Parse(content).RootElement;
            var role = root.EnumerateArray().FirstOrDefault(r => r.GetProperty("name").GetString() == roleName);
            return role.GetProperty("id").GetString() ?? throw new Exception("Role ID missing.");
        }

        public async Task<bool> AssignRoleAsync(HttpClient client, string userId, string clientId, string roleId, string roleName)
        {
            var endpoint = _urlHelperKeycloak.GetRoleMappingsEndpoint(_configuration, userId, clientId);
            var response = await client.PostAsync(endpoint, _keycloakRequestBuilder.BuildJson(_keycloakRequestBuilder.GetRoleData(roleId, roleName)));

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Assign role failed: {await response.Content.ReadAsStringAsync()}");

            return true;
        }

        public async Task<bool> VerifyRoleAssignmentAsync(HttpClient client, string userId, string clientId, string roleId)
        {
            var endpoint = _urlHelperKeycloak.GetRoleMappingsEndpoint(_configuration, userId, clientId);
            var response = await client.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return false;
            var roles = JsonDocument.Parse(content).RootElement;
            return roles.EnumerateArray().Any(r => r.GetProperty("id").GetString() == roleId);
        }

        public async Task<bool> ChangeUserPasswordAsync(HttpClient client, string userId, string newPassword, bool temporary)
        {
            var endpoint = _urlHelperKeycloak.GetResetPasswordEndpoint(_configuration, userId);
            _keycloakRequestBuilder.WithNewPassword(newPassword);

            var passwordContent = _keycloakRequestBuilder.BuildJson(_keycloakRequestBuilder.GetNewPasswordData());
            var response = await client.PutAsync(endpoint, passwordContent);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException($"Error al cambiar la contraseña: {content}");
            }

            return true;
        }

        public async Task<bool> ResetPasswordAsync(HttpClient client, string userId, string newPassword, bool temporary)
        {
            try
            {
                var resetPasswordEndpoint = _urlHelperKeycloak.GetResetPasswordEndpoint(_configuration, userId);
                var resetPasswordRequest = _keycloakRequestBuilder
                    .WithNewPassword(newPassword)
                    .BuildJson(_keycloakRequestBuilder.GetNewPasswordData());

                var resetPasswordResponse = await client.PutAsync(resetPasswordEndpoint, resetPasswordRequest);
                var resetPasswordContent = await resetPasswordResponse.Content.ReadAsStringAsync();

                if (!resetPasswordResponse.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al resetear la contraseña: {resetPasswordContent}");
                }

                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new UnauthorizedAccessException($"Unauthorized access: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al resetear la contraseña: {ex.Message}");
            }
        }



        public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(HttpClient client, string refreshToken)
        {
            var endpoint = _urlHelperKeycloak.GetTokenUrl(_configuration);
            var form = new FormUrlEncodedContent(
                _keycloakRequestBuilder
                    .WithClientId()
                    .WithClientSecret()
                    .WithGrantType("refresh_token")
                    .WithRefreshToken(refreshToken)
                    .BuildAsForm()
            );

            var response = await client.PostAsync(endpoint, form);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedException("Token refresh failed.", new List<string> { content });

            var root = JsonDocument.Parse(content).RootElement;
            return (
                root.GetProperty("access_token").GetString() ?? throw new UnauthorizedAccessException("Missing access token."),
                root.GetProperty("refresh_token").GetString() ?? throw new UnauthorizedAccessException("Missing refresh token.")
            );
        }

        public async Task<bool> LogoutAsync(HttpClient client, IConfiguration configuration, string refreshToken)
        {
            var endpoint = _urlHelperKeycloak.GetLogoutUrl(configuration);
            var form = new FormUrlEncodedContent(
                _keycloakRequestBuilder
                    .WithRefreshToken(refreshToken)
                    .WithClientId()
                    .WithClientSecret()
                    .BuildAsForm()
            );

            var response = await client.PostAsync(endpoint, form);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Logout failed: {content}");

            return true;
        }
    }
}
