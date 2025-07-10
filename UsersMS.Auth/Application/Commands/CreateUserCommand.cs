using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Application.DTOs.Auth;

namespace UsersMS.Auth.Application.Commands
{
    public class CreateUserCommand : IRequest<CreateUserResponseDTO>
    {
        public CreateUserRequestDTO CreateUserRequestDTO { get; set; }

        public CreateUserCommand(CreateUserRequestDTO createUserRequestDTO)
        {
            CreateUserRequestDTO = createUserRequestDTO;
        }
    }
}
