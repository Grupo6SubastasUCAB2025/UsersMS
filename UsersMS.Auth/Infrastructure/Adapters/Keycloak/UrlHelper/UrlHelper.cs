using Microsoft.Extensions.Configuration;
using UsersMS.Commons.Exceptions;

namespace UsersMS.Infrastructure.Adapters.Keycloak.UrlHelper
{
    public class UrlHelperKeycloak : IUrlHelperKeycloak
    {
        public string GetBaseUrl(IConfiguration configuration)
        {
            var protocol = configuration["Keycloak:ProtocolhttpPath"];
            var host = configuration["Keycloak:Host"];
            var port = configuration["Keycloak:Port"];
            if (string.IsNullOrEmpty(protocol) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
                throw new ConfigurationException("Protocol, Host or Port configuration is missing for Keycloak.");

            return $"{protocol}{host}:{port}";
        }

        public string GetRealmUrl(IConfiguration configuration)
        {
            var baseUrl = GetBaseUrl(configuration);
            var realmPath = configuration["Keycloak:RealmPath"];
            var realm = configuration["Keycloak:Realm"];
            if (string.IsNullOrEmpty(realmPath) || string.IsNullOrEmpty(realm))
                throw new ConfigurationException("RealmPath or Realm configuration is missing for Keycloak.");

            return $"{baseUrl}{realmPath}{realm}";
        }

        public string GetAdminUrl(IConfiguration configuration)
        {
            var baseUrl = GetBaseUrl(configuration);
            var adminPath = configuration["Keycloak:AdminPath"];
            var realmPath = configuration["Keycloak:RealmPath"];
            var realm = configuration["Keycloak:Realm"];
            if (string.IsNullOrEmpty(adminPath) || string.IsNullOrEmpty(realm))
                throw new ConfigurationException("AdminPath or Realm configuration is missing for Keycloak.");

            return $"{baseUrl}{adminPath}{realmPath}{realm}";
        }

        private string GetProtocolUrl(IConfiguration configuration)
        {
            var realmUrl = GetRealmUrl(configuration);
            var openidConnectPath = configuration["Keycloak:OpenidConnectPath"];
            if (string.IsNullOrEmpty(openidConnectPath))
                throw new ConfigurationException("OpenidConnectPath configuration is missing for Keycloak.");

            return $"{realmUrl}{openidConnectPath}";
        }

        public string GetTokenUrl(IConfiguration configuration)
        {
            var protocolUrl = GetProtocolUrl(configuration);
            var tokenPath = configuration["Keycloak:TokenPath"];
            if (string.IsNullOrEmpty(tokenPath))
                throw new ConfigurationException("TokenPath configuration is missing for Keycloak.");

            return $"{protocolUrl}{tokenPath}";
        }

        public string GetIntrospectUrl(IConfiguration configuration)
        {
            var tokenUrl = GetTokenUrl(configuration);
            var introspectPath = configuration["Keycloak:IntrospectPath"];
            if (string.IsNullOrEmpty(introspectPath))
                throw new ConfigurationException("IntrospectPath configuration is missing for Keycloak.");

            return $"{tokenUrl}{introspectPath}";
        }

        public string GetUserEndpoint(IConfiguration configuration)
        {
            var adminUrl = GetAdminUrl(configuration);
            var userPath = configuration["Keycloak:UserPath"];
            if (string.IsNullOrEmpty(userPath))
                throw new ConfigurationException("UserPath configuration is missing for Keycloak.");

            return $"{adminUrl}{userPath}";
        }

        public string GetUserByEmailUrl(IConfiguration configuration, string email)
        {
            var userEndpoint = GetUserEndpoint(configuration);
            if (string.IsNullOrEmpty(email))
                throw new ConfigurationException("Email cannot be null or empty.");

            return $"{userEndpoint}?email={email}";
        }

        public string GetUserByIdEndpoint(IConfiguration configuration, string userId)
        {
            var userEndpoint = GetUserEndpoint(configuration);
            if (string.IsNullOrEmpty(userId))
                throw new ConfigurationException("UserId cannot be null or empty.");

            return $"{userEndpoint}/{userId}";
        }

        public string GetLogoutUrl(IConfiguration configuration)
        {
            var protocolUrl = GetProtocolUrl(configuration);
            var logoutPath = configuration["Keycloak:LogoutPath"];
            if (string.IsNullOrEmpty(logoutPath))
                throw new ConfigurationException("LogoutPath configuration is missing for Keycloak.");

            return $"{protocolUrl}{logoutPath}";
        }
        public string GetClientsEndpoint(IConfiguration configuration)
        {
            var adminUrl = GetAdminUrl(configuration);
            var clientsPath = configuration["Keycloak:ClientsPath"];
            if (string.IsNullOrEmpty(clientsPath))
            {
                throw new ConfigurationException("ClientsPath configuration is missing for Keycloak.");
            }
            return $"{adminUrl}{clientsPath}";
        }

        public string GetClientRolesEndpoint(IConfiguration configuration, string clientId)
        {
            var adminUrl = GetAdminUrl(configuration);
            var clientsPath = configuration["Keycloak:ClientsPath"];
            var rolesPath = configuration["Keycloak:RolesPath"];
            if (string.IsNullOrEmpty(clientsPath) || string.IsNullOrEmpty(rolesPath))
            {
                throw new ConfigurationException("ClientsPath or RolesPath configuration is missing for Keycloak.");
            }
            return $"{adminUrl}{clientsPath}/{clientId}{rolesPath}";
        }

        public string GetRoleMappingsEndpoint(IConfiguration configuration, string userId, string clientId)
        {
            var userByIdEndpoint = GetUserByIdEndpoint(configuration, userId);
            var roleMappingsPath = configuration["Keycloak:RoleMappings"];
            var clientsPath = configuration["Keycloak:ClientsPath"];
            if (string.IsNullOrEmpty(roleMappingsPath) || string.IsNullOrEmpty(clientsPath))
            {
                throw new ConfigurationException("RoleMappings or ClientsPath configuration is missing for Keycloak.");
            }
            return $"{userByIdEndpoint}{roleMappingsPath}{clientsPath}/{clientId}";
        }

        public string GetResetPasswordEndpoint(IConfiguration configuration, string userId)
        {
            var userByIdEndpoint = GetUserByIdEndpoint(configuration, userId);
            var resetPasswordPath = configuration["Keycloak:ResetPasswordPath"];
            if (string.IsNullOrEmpty(resetPasswordPath))
            {
                throw new ConfigurationException("ResetPasswordPath configuration is missing for Keycloak.");
            }
            return $"{userByIdEndpoint}{resetPasswordPath}";
        }
    }
}
