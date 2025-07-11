using System;
using System.Threading.Tasks;
using UsersMS.Application.DTOs.Auth;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.Adapters;
using UsersMS.Infrastructure.Adapters.Keycloak;
using UsersMS.Domain.Utilities;
using System.Net.Http;
using UsersMS.Auth.Infrastructure.DTOs.ChangePassword;

namespace UsersMS.Auth.Infrastructure.Validators.ChangePassword
{
    public class AuthChangePasswordValidator : IService<ChangePasswordRequestDTO, ChangePasswordResponseDTO>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HeadersToken _headersToken;
        private readonly IKeycloakRepository _keycloakRepository;

        public AuthChangePasswordValidator(
            IHttpClientFactory httpClientFactory,
            HeadersToken headersToken,
            IKeycloakRepository keycloakRepository)
        {
            _httpClientFactory = httpClientFactory;
            _headersToken = headersToken;
            _keycloakRepository = keycloakRepository;
        }

        public async Task<ChangePasswordResponseDTO> Execute(ChangePasswordRequestDTO request)
        {
            var client = _httpClientFactory.CreateClient();
            var temporaryPassword = false;

            try
            {
                var token = _headersToken.GetToken();
                _headersToken.SetAuthorizationHeader(client);

                var (userId, _, email) = await _keycloakRepository.IntrospectTokenAsync(client, token);

                if (!string.Equals(email, request.UserEmail, StringComparison.OrdinalIgnoreCase))
                {
                    return new ChangePasswordResponseDTO
                    {
                        Success = false,
                        Message = "El email del token no coincide con el enviado.",
                        Time = DateTime.UtcNow,
                        UserEmail = request.UserEmail
                    };
                }

                var success = await _keycloakRepository.ChangeUserPasswordAsync(client, userId, request.NewPassword, temporaryPassword);
                if (!success)
                {
                    return new ChangePasswordResponseDTO
                    {
                        Success = false,
                        Message = "No se pudo cambiar la contraseña.",
                        Time = DateTime.UtcNow,
                        UserEmail = request.UserEmail
                    };
                }

                return new ChangePasswordResponseDTO
                {
                    Success = true,
                    Message = "Contraseña actualizada exitosamente.",
                    Time = DateTime.UtcNow,
                    TemporaryPassword = temporaryPassword,
                    UserEmail = request.UserEmail
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                return new ChangePasswordResponseDTO
                {
                    Success = false,
                    Message = $"Acceso no autorizado: {ex.Message}",
                    Time = DateTime.UtcNow,
                    UserEmail = request.UserEmail
                };
            }
            catch (Exception ex)
            {
                return new ChangePasswordResponseDTO
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Time = DateTime.UtcNow,
                    UserEmail = request.UserEmail
                };
            }
        }
    }
}
