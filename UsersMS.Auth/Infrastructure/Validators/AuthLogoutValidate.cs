using Microsoft.Extensions.Configuration;
using UsersMS.Auth.Infrastructure.DTOs.Logout;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.Adapters;
using UsersMS.Infrastructure.Adapters.Keycloak;

namespace UsersMS.Application.Validators.Logout
{
    public class AuthLogoutValidate : IService<LogoutRequestDTO, LogoutResponseDTO>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IKeycloakRepository _keycloakRepository;
        private readonly HeadersToken _headersToken;

        public AuthLogoutValidate(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IKeycloakRepository keycloakRepository,
            HeadersToken headersToken)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _keycloakRepository = keycloakRepository;
            _headersToken = headersToken;
        }

        public async Task<LogoutResponseDTO> Execute(LogoutRequestDTO request)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var success = await _keycloakRepository.LogoutAsync(client, _configuration, request.RefreshToken);

                _headersToken.SetAuthorizationHeader(client);
                var token = _headersToken.GetToken();

                var (_, _, _) = await _keycloakRepository.IntrospectTokenAsync(client, token);

                return new LogoutResponseDTO
                {
                    Success = false,
                    Message = "Logout fallido: el token aún está activo.",
                    UserEmail = request.UserEmail,
                    Time = DateTime.UtcNow,
                    Active = true
                };
            }
            catch (UnauthorizedAccessException)
            {
                return new LogoutResponseDTO
                {
                    Success = true,
                    Message = "Logout exitoso.",
                    UserEmail = request.UserEmail,
                    Time = DateTime.UtcNow,
                    Active = false
                };
            }
            catch (Exception ex)
            {
                return new LogoutResponseDTO
                {
                    Success = false,
                    Message = ex.Message,
                    UserEmail = request.UserEmail,
                    Time = DateTime.UtcNow,
                    Active = true
                };
            }
        }
    }
}
