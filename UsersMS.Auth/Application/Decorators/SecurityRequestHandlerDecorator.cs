using MediatR;
using Microsoft.AspNetCore.Http;
using UsersMS.Commons.Exceptions;
using UsersMS.Infrastructure.Adapters;
using UsersMS.Infrastructure.Adapters.Keycloak;

namespace UsersMS.Application.Decorators
{
    public class SecurityRequestHandlerDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _innerHandler;
        private readonly IKeycloakRepository _keycloakRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HeadersToken _headersToken;
        private readonly string[] _allowedRoles;

        public SecurityRequestHandlerDecorator(
            IRequestHandler<TRequest, TResponse> innerHandler,
            IKeycloakRepository keycloakRepository,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            params string[] allowedRoles)
        {
            _innerHandler = innerHandler ?? throw new ArgumentNullException(nameof(innerHandler));
            _keycloakRepository = keycloakRepository ?? throw new ArgumentNullException(nameof(keycloakRepository));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _headersToken = new HeadersToken(httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor)));
            _allowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var token = _headersToken.GetToken();
            var client = _httpClientFactory.CreateClient();
            var (userId, role, email) = await _keycloakRepository.IntrospectTokenAsync(client, token);

            if (!_allowedRoles.Contains(role))
            {
                throw new UnauthorizedException("Unauthorized access: role validation failed.");
            }

            return await _innerHandler.Handle(request, cancellationToken);
        }
    }
}
