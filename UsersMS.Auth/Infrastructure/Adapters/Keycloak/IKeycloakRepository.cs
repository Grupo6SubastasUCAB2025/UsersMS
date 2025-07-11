using Microsoft.Extensions.Configuration;

namespace UsersMS.Infrastructure.Adapters.Keycloak
{
    public interface IKeycloakRepository
    {
        Task<(string AccessToken, string RefreshToken)> GetTokenAsync(HttpClient client, string email, string password);
        Task<string> GetClientCredentialsTokenAsync(HttpClient client);
        Task<(string UserId, string Role, string Email)> IntrospectTokenAsync(HttpClient client, string token);
        Task<bool> CreateUserAsync(HttpClient client, string email, string password, Dictionary<string, string> attributes);

        Task<(string UserId, bool HasRequiredAction)> GetUserByEmailAsync(HttpClient client, string email, string requiredAction);
        Task<string> GetClientIdAsync(HttpClient client);
        Task<string> GetRoleAsync(HttpClient client, string clientId, string roleName);
        Task<bool> AssignRoleAsync(HttpClient client, string userId, string clientId, string roleId, string roleName);
        Task<bool> VerifyRoleAssignmentAsync(HttpClient client, string userId, string clientId, string roleId);
        Task<bool> ChangeUserPasswordAsync(HttpClient client, string userId, string newPassword, bool temporary);
        Task<bool> ResetPasswordAsync(HttpClient client, string userId, string newPassword, bool temporary);
        Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(HttpClient client, string refreshToken);
        Task<bool> LogoutAsync(HttpClient client, IConfiguration configuration, string refreshToken);
    }

}
