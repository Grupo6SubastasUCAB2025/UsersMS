using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace UsersMS.Infrastructure.Adapters
{
    public class HeadersToken
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeadersToken(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken(string? fallbackToken = null)
        {
            var context = _httpContextAccessor.HttpContext;

            var authorizationHeader = context?.Request?.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                var token = authorizationHeader.Split(" ").Last();
                if (!string.IsNullOrEmpty(token))
                {
                    return token;
                }
            }

            if (!string.IsNullOrEmpty(fallbackToken))
            {
                return fallbackToken;
            }

            throw new InvalidOperationException("Authorization header y fallback token están ausentes.");
        }

        public void SetAuthorizationHeader(HttpClient client, string? fallbackToken = null)
        {
            var token = GetToken(fallbackToken);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
