using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Application.DTOs.Auth;
using UsersMS.Auth.Infrastructure.DTOs.Login;

namespace UsersMS.Auth.Application.Commands
{
    public class LoginCommand : IRequest<LoginResponseDTO>
    {
        public LoginRequestDTO LoginRequestDTO { get; set; }

        public LoginCommand(LoginRequestDTO loginRequestDTO)
        {
            LoginRequestDTO = loginRequestDTO;
        }
    }
}
