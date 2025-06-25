namespace UsersMS.Infrastructure.Adapters
{
    public interface IHeadersClientCredentialsToken
    {
        Task SetClientCredentialsToken(HttpClient client);
    }
}
