using Microsoft.Extensions.Configuration;

namespace UsersMS.Infrastructure.Adapters.Keycloak
{
    public interface IKeycloakRepository
    {
        Task<(string AccessToken, string RefreshToken)> GetTokenAsync(HttpClient client, string email, string password);

        Task<string> GetClientCredentialsTokenAsync(HttpClient client);

        Task<(string UserId, string Email)> IntrospectTokenAsync(HttpClient client, string token);

        Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(HttpClient client, string refreshToken);

        Task<bool> CreateUserAsync(HttpClient client, string email, string password);

        Task<string> GetUserIdByEmailAsync(HttpClient client, string email);
       
        Task<bool> LogoutAsync(HttpClient client, IConfiguration configuration, string refreshToken);
    }

}
