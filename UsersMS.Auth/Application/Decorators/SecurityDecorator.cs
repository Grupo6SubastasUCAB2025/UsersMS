using Microsoft.AspNetCore.Http;
using UsersMS.Commons.Exceptions;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.Adapters;
using UsersMS.Infrastructure.Adapters.Keycloak;

namespace UsersMS.Application.Decorators
{
    public class SecurityDecorator<TRequest, TResponse> : IService<TRequest, TResponse>
    {
        private readonly IService<TRequest, TResponse> _innerService;
        private readonly IKeycloakRepository _keycloakRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HeadersToken _headersToken;
        private readonly string[] _allowedRoles;

        public SecurityDecorator(
            IService<TRequest, TResponse> innerService,
            IKeycloakRepository keycloakRepository,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            params string[] allowedRoles)
        {
            _innerService = innerService ?? throw new ArgumentNullException(nameof(innerService));
            _keycloakRepository = keycloakRepository ?? throw new ArgumentNullException(nameof(keycloakRepository));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _headersToken = new HeadersToken(httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor)));
            _allowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
        }

        public async Task<TResponse> Execute(TRequest request)
        {
            var token = _headersToken.GetToken();
            var client = _httpClientFactory.CreateClient();
            var (userId, role, email) = await _keycloakRepository.IntrospectTokenAsync(client, token);

            if (email != (request as dynamic)?.UserEmail || userId != (request as dynamic)?.UserId.ToString())
            {
                throw new UnauthorizedException("Unauthorized access: token validation failed.");
            }

            if (!_allowedRoles.Contains(role))
            {
                throw new UnauthorizedException("Unauthorized access: role validation failed.");
            }

            return await _innerService.Execute(request);
        }
    }
}
