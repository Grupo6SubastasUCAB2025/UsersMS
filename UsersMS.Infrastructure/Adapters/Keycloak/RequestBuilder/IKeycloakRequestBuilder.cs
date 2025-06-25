namespace UsersMS.Infrastructure.Adapters.Keycloak.RequestBuilder
{
    public interface IKeycloakRequestBuilder
    {
        IKeycloakRequestBuilder WithToken(string token);
        IKeycloakRequestBuilder WithRefreshToken(string refreshToken);
        IKeycloakRequestBuilder WithClientId();
        IKeycloakRequestBuilder WithClientSecret();
        IKeycloakRequestBuilder WithUsername(string username);
        IKeycloakRequestBuilder WithEmail(string email);
        IKeycloakRequestBuilder WithPassword(string password);
        IKeycloakRequestBuilder WithGrantType(string grantType);
        IKeycloakRequestBuilder WithEmailVerified(bool emailVerified);
        IKeycloakRequestBuilder WithEnabled(bool enabled);
        object GetUserData();
        IEnumerable<KeyValuePair<string, string>> BuildAsForm();
        StringContent BuildJson(object data);
    }
}
