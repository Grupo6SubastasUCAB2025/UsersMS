using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Auth.Infrastructure.DTOs.Logout;

namespace UsersMS.Auth.Application.Commands
{
    public class LogoutCommand : IRequest<LogoutResponseDTO>
    {
        public LogoutRequestDTO LogoutRequestDTO { get; set; }

        public LogoutCommand(LogoutRequestDTO logoutRequestDTO)
        {
            LogoutRequestDTO = logoutRequestDTO;
        }
    }
}
