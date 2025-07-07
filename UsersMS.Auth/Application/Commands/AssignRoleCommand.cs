using MediatR;
using UsersMS.Application.DTOs.Auth;

namespace UsersMS.Application.Commands.Auth
{
    public class AssignRoleCommand : IRequest<AssignRoleResponseDTO>
    {
        public AssignRoleRequestDTO AssignRoleRequest { get; set; }

        public AssignRoleCommand(AssignRoleRequestDTO assignRoleRequest)
        {
            AssignRoleRequest = assignRoleRequest;
        }
    }
}

