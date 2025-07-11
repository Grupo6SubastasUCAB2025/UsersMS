using System.Net.Http.Headers;
using UsersMS.Application.DTOs.Auth;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.Adapters.Keycloak.Email;
using UsersMS.Infrastructure.Adapters.Keycloak;
using UsersMS.Infrastructure.Adapters;
using Microsoft.AspNetCore.Http;

namespace UsersMS.Application.Validators.CreateUser
{
    public class AuthCreateUserValidator : IService<CreateUserRequestDTO, CreateUserResponseDTO>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IKeycloakRepository _keycloakRepository;
        private readonly IService<AssignRoleRequestDTO, AssignRoleResponseDTO> _assignRoleService;
        private readonly EmailProcessor _emailProcessor;

        public AuthCreateUserValidator(
            IHttpClientFactory httpClientFactory,
            IKeycloakRepository keycloakRepository,
            IService<AssignRoleRequestDTO, AssignRoleResponseDTO> assignRoleService,
            EmailProcessor emailProcessor)
        {
            _httpClientFactory = httpClientFactory;
            _keycloakRepository = keycloakRepository;
            _assignRoleService = assignRoleService;
            _emailProcessor = emailProcessor;
        }

        public async Task<CreateUserResponseDTO> Execute(CreateUserRequestDTO request)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();

            try
            {
                var token = await _keycloakRepository.GetClientCredentialsTokenAsync(client);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (string.IsNullOrEmpty(request.NameRole))
                {
                    return new CreateUserResponseDTO
                    {
                        Success = false,
                        Message = "RoleName is required.",
                        Time = DateTime.UtcNow,
                        Email = request.UserEmail
                    };
                }

                var userId = Guid.NewGuid();

                var attributes = new Dictionary<string, string>
                {
                    { "userId", userId.ToString() },
                    { "name", request.Name },
                    { "cedula", request.Cedula },
                    { "phone", request.Phone }
                };

                var userCreated = await _keycloakRepository.CreateUserAsync(
                    client,
                    request.UserEmail,
                    request.Password,
                    attributes
                );

                if (!userCreated)
                {
                    return new CreateUserResponseDTO
                    {
                        Success = false,
                        Message = "Error creating user.",
                        Time = DateTime.UtcNow,
                        Email = request.UserEmail
                    };
                }

                var assignRoleResponse = await _assignRoleService.Execute(new AssignRoleRequestDTO
                {
                    RoleName = request.NameRole,
                    UserEmail = request.UserEmail
                });

                if (!assignRoleResponse.Success)
                {
                    return new CreateUserResponseDTO
                    {
                        Success = false,
                        Message = "Error assigning role.",
                        Time = DateTime.UtcNow,
                        Email = request.UserEmail
                    };
                }

                /*
                var emailResponse = await _emailProcessor.SendEmailAsync(
                    request.UserEmail,
                    "Account Created",
                    "new-user.ftl",
                    new Dictionary<string, string> { { "password", request.Password } });

                if (!emailResponse.Success)
                {
                    return new CreateUserResponseDTO
                    {
                        Success = false,
                        Message = emailResponse.Message,
                        Time = DateTime.UtcNow,
                        Email = request.UserEmail
                    };
                }
                */

                return new CreateUserResponseDTO
                {
                    Success = true,
                    Message = "User created successfully.",
                    Time = DateTime.UtcNow,
                    Email = request.UserEmail,
                    NameRole = request.NameRole,
                    UserId = userId
                };
            }
            catch (Exception ex)
            {
                return new CreateUserResponseDTO
                {
                    Success = false,
                    Message = $"Error interno: {ex.Message}",
                    Time = DateTime.UtcNow,
                    Email = request.UserEmail
                };
            }
        }
    }
}
