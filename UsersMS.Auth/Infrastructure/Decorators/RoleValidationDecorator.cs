using Microsoft.AspNetCore.Http;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.Adapters.Keycloak;
using UsersMS.Infrastructure.Adapters;
using UsersMS.Commons.Exceptions;
using UsersMS.Domain.Utilities;

namespace UsersMS.Infrastructure.Decorators
{
    public class RoleValidationDecorator<TRequest, TResponse> : IService<TRequest, TResponse>
    {
        private readonly IService<TRequest, TResponse> _innerService;
        private readonly IKeycloakRepository _keycloakRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HeadersToken _headersToken;

        public RoleValidationDecorator(
            IService<TRequest, TResponse> innerService,
            IKeycloakRepository keycloakRepository,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _innerService = innerService ?? throw new ArgumentNullException(nameof(innerService));
            _keycloakRepository = keycloakRepository ?? throw new ArgumentNullException(nameof(keycloakRepository));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _headersToken = new HeadersToken(httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor)));
        }

        public async Task<TResponse> Execute(TRequest request)
        {
            var token = _headersToken.GetToken();
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedException("Authorization header is missing.");
            }

            var client = _httpClientFactory.CreateClient();
            var (userId, role, email) = await _keycloakRepository.IntrospectTokenAsync(client, token);

            var requestDto = request as dynamic;
            if (requestDto == null)
            {
                throw new InvalidOperationException("Invalid request type.");
            }

            if (email != requestDto.UserEmail || userId != requestDto.UserId.ToString())
            {
                throw new UnauthorizedException("Unauthorized access: UserId or UserEmail validation failed.");
            }

            if (!RoleValidator.CanPerformAction(role, requestDto.NameRole))
            {
                throw new UnauthorizedException("Unauthorized access: role validation failed.");
            }

            return await _innerService.Execute(request);
        }
    }
}
