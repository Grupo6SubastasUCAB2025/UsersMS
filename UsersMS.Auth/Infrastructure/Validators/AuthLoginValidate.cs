using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using UsersMS.Application.DTOs.Auth;
using UsersMS.Auth.Infrastructure.DTOs.Login;
using UsersMS.Commons.Exceptions;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.Adapters.Keycloak;

namespace UsersMS.Application.Validators.Login
{
    public class AuthLoginValidate : IService<LoginRequestDTO, LoginResponseDTO>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IKeycloakRepository _keycloakRepository;

        public AuthLoginValidate(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IKeycloakRepository keycloakRepository)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _keycloakRepository = keycloakRepository;
        }

        public async Task<LoginResponseDTO> Execute(LoginRequestDTO request)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var (accessToken, refreshToken) = await _keycloakRepository.GetTokenAsync(client, request.UserEmail, request.Password);

                var authType = _configuration["Keycloak:Auth_Type"];
                if (string.IsNullOrEmpty(authType))
                {
                    throw new ConfigurationException("Falta la configuración 'Auth_Type' para JwtBearer.");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authType, accessToken);

                var (userIdString, role, _) = await _keycloakRepository.IntrospectTokenAsync(client, accessToken);

                if (!Guid.TryParse(userIdString, out Guid userId))
                {
                    throw new ArgumentException("El ID del usuario no tiene un formato válido.");
                }

                return new LoginResponseDTO
                {
                    Success = true,
                    Message = "Inicio de sesión exitoso",
                    Time = DateTime.UtcNow,
                    UserEmail = request.UserEmail,
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    UserID = userId.ToString(),
                    Role = role
                };
            }
            catch (UnauthorizedException ex)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = $"Acceso no autorizado: {ex.Message}",
                    UserEmail = request.UserEmail,
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new LoginResponseDTO
                {
                    Success = false,
                    Message = ex.Message,
                    UserEmail = request.UserEmail,
                    Time = DateTime.UtcNow
                };
            }
        }
    }
}
