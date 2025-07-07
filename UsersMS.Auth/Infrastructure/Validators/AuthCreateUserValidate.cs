using UsersMS.Application.DTOs.Auth;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.Adapters.Keycloak.Email;
using UsersMS.Infrastructure.Adapters.Keycloak;
using UsersMS.Infrastructure.Adapters;

namespace UsersMS.Application.Validators.CreateUser
{
    public class AuthCreateUserValidator : IService<CreateUserRequestDTO, CreateUserResponseDTO>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HeadersToken _headersToken;
        private readonly IKeycloakRepository _keycloakRepository;
        private readonly IService<AssignRoleRequestDTO, AssignRoleResponseDTO> _assignRoleService;
        private readonly EmailProcessor _emailProcessor;

        public AuthCreateUserValidator(
            IHttpClientFactory httpClientFactory,
            HeadersToken headersToken,
            IKeycloakRepository keycloakRepository,
            IService<AssignRoleRequestDTO, AssignRoleResponseDTO> assignRoleService,
            EmailProcessor emailProcessor)
        {
            _httpClientFactory = httpClientFactory;
            _headersToken = headersToken;
            _keycloakRepository = keycloakRepository;
            _assignRoleService = assignRoleService;
            _emailProcessor = emailProcessor;
        }

        public async Task<CreateUserResponseDTO> Execute(CreateUserRequestDTO request)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var token = _headersToken.GetToken();
                _headersToken.SetAuthorizationHeader(client);

                if (string.IsNullOrEmpty(request.NameRole))
                {
                    return new CreateUserResponseDTO
                    {
                        Success = false,
                        Message = "RoleName is required.",
                        Time = DateTime.UtcNow,
                        UserEmail = request.UserEmail
                    };
                }

                var userCreated = await _keycloakRepository.CreateUserAsync(client, request.EmailToCreate, request.Password);
                if (!userCreated)
                {
                    return new CreateUserResponseDTO
                    {
                        Success = false,
                        Message = "Error creating user.",
                        Time = DateTime.UtcNow,
                        UserEmail = request.UserEmail,
                        EmailToCreate = request.EmailToCreate
                    };
                }

                var (userIdStr, _) = await _keycloakRepository.GetUserByEmailAsync(client, request.EmailToCreate, string.Empty);
                if (!Guid.TryParse(userIdStr, out var userId))
                {
                    return new CreateUserResponseDTO
                    {
                        Success = false,
                        Message = "Error retrieving user ID.",
                        Time = DateTime.UtcNow,
                        UserEmail = request.UserEmail,
                        EmailToCreate = request.EmailToCreate
                    };
                }

                var assignRoleResponse = await _assignRoleService.Execute(new AssignRoleRequestDTO
                {
                    EmailAssignedRole = request.EmailToCreate,
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
                        UserEmail = request.UserEmail
                    };
                }

                var emailResponse = await _emailProcessor.SendEmailAsync(
                    request.EmailToCreate,
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
                        UserEmail = request.UserEmail
                    };
                }

                return new CreateUserResponseDTO
                {
                    Success = true,
                    Message = "User created successfully.",
                    Time = DateTime.UtcNow,
                    UserEmail = request.UserEmail,
                    EmailToCreate = request.EmailToCreate,
                    NameRole = request.NameRole
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                return new CreateUserResponseDTO
                {
                    Success = false,
                    Message = $"Unauthorized: {ex.Message}",
                    Time = DateTime.UtcNow,
                    UserEmail = request.UserEmail
                };
            }
            catch (Exception ex)
            {
                return new CreateUserResponseDTO
                {
                    Success = false,
                    Message = ex.Message,
                    Time = DateTime.UtcNow,
                    UserEmail = request.UserEmail
                };
            }
        }
    }
}
