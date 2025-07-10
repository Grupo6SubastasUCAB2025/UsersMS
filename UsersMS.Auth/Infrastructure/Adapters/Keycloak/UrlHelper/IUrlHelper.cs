using Microsoft.Extensions.Configuration;

namespace UsersMS.Infrastructure.Adapters.Keycloak.UrlHelper
{
    public interface IUrlHelperKeycloak
    {
        string GetBaseUrl(IConfiguration configuration);
        string GetRealmUrl(IConfiguration configuration);
        string GetTokenUrl(IConfiguration configuration);
        string GetIntrospectUrl(IConfiguration configuration);
        string GetUserEndpoint(IConfiguration configuration);
        string GetUserByEmailUrl(IConfiguration configuration, string email);
        string GetUserByIdEndpoint(IConfiguration configuration, string userId);
        string GetLogoutUrl(IConfiguration configuration);
        string GetClientRolesEndpoint(IConfiguration configuration, string clientId);
        string GetRoleMappingsEndpoint(IConfiguration configuration, string userId, string clientId);
        string GetClientsEndpoint(IConfiguration configuration);
        string GetResetPasswordEndpoint(IConfiguration configuration, string userId);


    }
}
