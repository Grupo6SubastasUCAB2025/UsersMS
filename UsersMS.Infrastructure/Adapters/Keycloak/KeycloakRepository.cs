using Microsoft.Extensions.Configuration;
using System.Text.Json;
using UsersMS.Infrastructure.Adapters.Keycloak.RequestBuilder;
using UsersMS.Infrastructure.Adapters.Keycloak.UrlHelper;

namespace UsersMS.Infrastructure.Adapters.Keycloak
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

                var formContent = new FormUrlEncodedContent(tokenRequest);
                var response = await client.PostAsync(tokenEndpoint, formContent);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = JsonDocument.Parse(content).RootElement.GetProperty("error_description").GetString();
                    var error = JsonDocument.Parse(content).RootElement.GetProperty("error").GetString();

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest && error == "invalid_grant" && errorDetails == "Account is not fully set up")
                        throw new UnauthorizedAccessException("La cuenta no está completamente configurada");

                    if ((response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.Unauthorized) && error == "invalid_grant")
                        throw new UnauthorizedAccessException("Credenciales inválidas");

                    throw new UnauthorizedAccessException("Credenciales inválidas: " + (errorDetails ?? "Sin detalles adicionales."));
                }

                var root = JsonDocument.Parse(content).RootElement;

                var accessToken = root.GetProperty("access_token").GetString() ?? throw new UnauthorizedAccessException("El token de acceso es nulo.");
                var refreshToken = root.GetProperty("refresh_token").GetString() ?? throw new UnauthorizedAccessException("El token de refresco es nulo.");

                return (accessToken, refreshToken);
            }

            public async Task<string> GetClientCredentialsTokenAsync(HttpClient client)
            {
                var tokenEndpoint = _urlHelperKeycloak.GetTokenUrl(_configuration);

                var tokenRequest = _keycloakRequestBuilder
                    .WithClientId()
                    .WithClientSecret()
                    .WithGrantType("client_credentials")
                    .BuildAsForm();

                var formContent = new FormUrlEncodedContent(tokenRequest);
                var response = await client.PostAsync(tokenEndpoint, formContent);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = JsonDocument.Parse(content).RootElement.GetProperty("error_description").GetString();
                    throw new UnauthorizedAccessException("Error al obtener token de credenciales de cliente: " + (errorDetails ?? "Sin detalles adicionales."));
                }

                var root = JsonDocument.Parse(content).RootElement;
                return root.GetProperty("access_token").GetString() ?? throw new UnauthorizedAccessException("El token de acceso es nulo.");
            }

            public async Task<(string UserId, string Email)> IntrospectTokenAsync(HttpClient client, string token)
            {
                var introspectEndpoint = _urlHelperKeycloak.GetIntrospectUrl(_configuration);

                var introspectRequest = _keycloakRequestBuilder
                    .WithToken(token)
                    .WithClientId()
                    .WithClientSecret()
                    .BuildAsForm();

                var response = await client.PostAsync(introspectEndpoint, new FormUrlEncodedContent(introspectRequest));
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = JsonDocument.Parse(content).RootElement.GetProperty("error_description").GetString();
                    throw new UnauthorizedAccessException($"Fallo en la introspección: {errorDetails ?? "Sin detalles"}");
                }

                var root = JsonDocument.Parse(content).RootElement;

                if (!root.GetProperty("active").GetBoolean())
                    throw new UnauthorizedAccessException("El token está inactivo.");

                var userId = root.GetProperty("sub").GetString() ?? throw new UnauthorizedAccessException("El UserId es nulo.");
                var email = root.GetProperty("email").GetString() ?? throw new UnauthorizedAccessException("El email es nulo.");

                return (userId, email);
            }

            public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(HttpClient client, string refreshToken)
            {
                var tokenEndpoint = _urlHelperKeycloak.GetTokenUrl(_configuration);

                var refreshRequest = _keycloakRequestBuilder
                    .WithClientId()
                    .WithClientSecret()
                    .WithGrantType("refresh_token")
                    .WithRefreshToken(refreshToken)
                    .BuildAsForm();

                var formContent = new FormUrlEncodedContent(refreshRequest);
                var response = await client.PostAsync(tokenEndpoint, formContent);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var jsonError = JsonDocument.Parse(content).RootElement;
                    string errorDescription = jsonError.TryGetProperty("error_description", out var desc) ? desc.GetString()! : "Sin detalles";
                    throw new UnauthorizedAccessException($"Error al refrescar token: {errorDescription}");
                }

                var json = JsonDocument.Parse(content).RootElement;

                var accessToken = json.GetProperty("access_token").GetString()
                    ?? throw new UnauthorizedAccessException("El token de acceso es nulo.");
                var newRefreshToken = json.GetProperty("refresh_token").GetString()
                    ?? throw new UnauthorizedAccessException("El token de refresco es nulo.");

                return (accessToken, newRefreshToken);
            }


            public async Task<bool> CreateUserAsync(HttpClient client, string email, string password)
            {
                var userEndpoint = _urlHelperKeycloak.GetUserEndpoint(_configuration);

                _keycloakRequestBuilder
                    .WithUsername(email)
                    .WithEmail(email)
                    .WithEnabled(true)
                    .WithEmailVerified(true);

                var userData = _keycloakRequestBuilder.GetUserData();
                var userRequestContent = _keycloakRequestBuilder.BuildJson(userData);

                var response = await client.PostAsync(userEndpoint, userRequestContent);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error al crear usuario: {content}");

                return true;
            }

            public async Task<string> GetUserIdByEmailAsync(HttpClient client, string email)
            {
                var userByEmailUrl = _urlHelperKeycloak.GetUserByEmailUrl(_configuration, email);

                var response = await client.GetAsync(userByEmailUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Error al obtener usuario por email: {content}");

                var json = JsonDocument.Parse(content);

                if (json.RootElement.ValueKind == JsonValueKind.Array && json.RootElement.GetArrayLength() > 0)
                {
                    var userId = json.RootElement[0].GetProperty("id").GetString();
                    if (string.IsNullOrEmpty(userId))
                        throw new Exception("El UserId es nulo o está vacío");

                    return userId;
                }

                throw new Exception("Usuario no encontrado.");
            }

            public async Task<bool> LogoutAsync(HttpClient client, IConfiguration configuration, string refreshToken)
            {
                var logoutUrl = _urlHelperKeycloak.GetLogoutUrl(configuration);
                var logoutRequest = _keycloakRequestBuilder
                    .WithRefreshToken(refreshToken)
                    .WithClientId()
                    .WithClientSecret()
                    .BuildAsForm();

                var formContent = new FormUrlEncodedContent(logoutRequest);
                var response = await client.PostAsync(logoutUrl, formContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Error al cerrar sesión: {responseContent}");
                }
                return true;
            }
    }
}
