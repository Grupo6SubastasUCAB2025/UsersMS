using UsersMS.Application.DTOs.Auth;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.Adapters.Keycloak;

namespace UsersMS.Infrastructure.Validators.AssignRole
{
    public class AssignRoleValidator : IService<AssignRoleRequestDTO, AssignRoleResponseDTO>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IKeycloakRepository _keycloakRepository;

        public AssignRoleValidator(
            IHttpClientFactory httpClientFactory,
            IKeycloakRepository keycloakRepository)
        {
            _httpClientFactory = httpClientFactory;
            _keycloakRepository = keycloakRepository;
        }

        public async Task<AssignRoleResponseDTO> Execute(AssignRoleRequestDTO request)
        {
            var client = _httpClientFactory.CreateClient();

            try
            {
                var token = await _keycloakRepository.GetClientCredentialsTokenAsync(client);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var clientId = await _keycloakRepository.GetClientIdAsync(client);
                if (string.IsNullOrEmpty(clientId))
                {
                    return Fail("The client ID could not be found.", request);
                }

                var roleId = await _keycloakRepository.GetRoleAsync(client, clientId, request.RoleName);
                if (string.IsNullOrEmpty(roleId))
                {
                    return Fail("The client role could not be found.", request);
                }

                var (userId, _) = await _keycloakRepository.GetUserByEmailAsync(client, request.UserEmail, string.Empty);

                if (await _keycloakRepository.VerifyRoleAssignmentAsync(client, userId, clientId, roleId))
                {
                    return Fail("The user already has a role assigned.", request);
                }

                if (!await _keycloakRepository.AssignRoleAsync(client, userId, clientId, roleId, request.RoleName))
                {
                    return Fail("The role could not be assigned to the user.", request);
                }

                if (!await _keycloakRepository.VerifyRoleAssignmentAsync(client, userId, clientId, roleId))
                {
                    return Fail("The role was not assigned correctly.", request);
                }

                return new AssignRoleResponseDTO
                {
                    Success = true,
                    Message = "Role assigned successfully",
                    Time = DateTime.UtcNow,
                    UserEmail = request.UserEmail,
                    RoleName = request.RoleName
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                return Fail($"Unauthorized access: {ex.Message}", request);
            }
            catch (Exception ex)
            {
                return Fail(ex.Message, request);
            }
        }

        private AssignRoleResponseDTO Fail(string message, AssignRoleRequestDTO request)
        {
            return new AssignRoleResponseDTO
            {
                Success = false,
                Message = message,
                Time = DateTime.UtcNow,
                UserEmail = request.UserEmail,
                RoleName = request.RoleName
            };
        }
    }
}
