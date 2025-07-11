using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UsersMS.Application.DTOs.Auth;
using UsersMS.Auth.Infrastructure.DTOs.RecoverPassword;
using UsersMS.Commons.Exceptions;
using UsersMS.Core.Application;
using UsersMS.Core.Utilities;
using UsersMS.Domain.Utilities;
using UsersMS.Infrastructure.Adapters;
using UsersMS.Infrastructure.Adapters.Keycloak;
using UsersMS.Infrastructure.Adapters.Keycloak.Email;

namespace UsersMS.Auth.Infrastructure.Validators.RecoverPassword
{
    public class AuthRecoverPasswordValidator : IService<RecoverPasswordRequestDTO, RecoverPasswordResponseDTO>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHeadersClientCredentialsToken _headersClientCredentialsToken;
        private readonly IKeycloakRepository _keycloakRepository;
        private readonly EmailProcessor _emailProcessor;

        public AuthRecoverPasswordValidator(
            IHttpClientFactory httpClientFactory,
            IHeadersClientCredentialsToken headersClientCredentialsToken,
            IKeycloakRepository keycloakRepository,
            EmailProcessor emailProcessor)
        {
            _httpClientFactory = httpClientFactory;
            _headersClientCredentialsToken = headersClientCredentialsToken;
            _keycloakRepository = keycloakRepository;
            _emailProcessor = emailProcessor;
        }

        public async Task<RecoverPasswordResponseDTO> Execute(RecoverPasswordRequestDTO request)
        {
            var client = _httpClientFactory.CreateClient();
            var temporaryPassword = true;

            try
            {
                await _headersClientCredentialsToken.SetClientCredentialsToken(client);

                var (userId, hasRequiredAction) = await _keycloakRepository.GetUserByEmailAsync(client, request.UserEmail, "UPDATE_PASSWORD");

                if (hasRequiredAction)
                {
                    return new RecoverPasswordResponseDTO
                    {
                        Success = false,
                        Message = "El usuario ya tiene una contraseña temporal pendiente.",
                        UserEmail = request.UserEmail,
                        Time = DateTime.UtcNow
                    };
                }

                var generatedPassword = PasswordGenerator.GeneratePassword();

                var passwordReset = await _keycloakRepository.ResetPasswordAsync(client, userId, generatedPassword, temporaryPassword);
                if (!passwordReset)
                {
                    return new RecoverPasswordResponseDTO
                    {
                        Success = false,
                        Message = "No se pudo reiniciar la contraseña.",
                        UserEmail = request.UserEmail,
                        Time = DateTime.UtcNow
                    };
                }

                var emailResponse = await _emailProcessor.SendEmailAsync(
                    request.UserEmail,
                    "Recuperación de Contraseña",
                    "temporary-password.ftl",
                    new Dictionary<string, string> { { "password", generatedPassword } });

                if (!emailResponse.Success)
                {
                    return new RecoverPasswordResponseDTO
                    {
                        Success = false,
                        Message = "Error al enviar el correo de recuperación.",
                        UserEmail = request.UserEmail,
                        Time = DateTime.UtcNow
                    };
                }

                Console.WriteLine($"\n\nGenerated temporary password: {generatedPassword}\n\n");

                return new RecoverPasswordResponseDTO
                {
                    Success = true,
                    Message = "Se ha enviado una contraseña temporal al correo.",
                    TemporaryPassword = temporaryPassword,
                    UserEmail = request.UserEmail,
                    Time = DateTime.UtcNow
                };
            }
            catch (UnauthorizedException ex)
            {
                return new RecoverPasswordResponseDTO
                {
                    Success = false,
                    Message = $"Acceso no autorizado: {ex.Message}",
                    UserEmail = request.UserEmail,
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new RecoverPasswordResponseDTO
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    UserEmail = request.UserEmail,
                    Time = DateTime.UtcNow
                };
            }
        }
    }
}
