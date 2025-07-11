using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Auth.Infrastructure.DTOs.ChangePassword;

namespace UsersMS.Auth.Application.Commands
{
    public class ChangePasswordCommand : IRequest<ChangePasswordResponseDTO>
    {
        public ChangePasswordRequestDTO ChangePasswordRequest { get; set; }

        public ChangePasswordCommand(ChangePasswordRequestDTO changePasswordRequest)
        {
            ChangePasswordRequest = changePasswordRequest;
        }
    }
}
