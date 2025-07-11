using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Auth.Infrastructure.DTOs.RecoverPassword;

namespace UsersMS.Auth.Application.Commands
{
    public class RecoverPasswordCommand : IRequest<RecoverPasswordResponseDTO>
    {
        public RecoverPasswordRequestDTO Request { get; set; }

        public RecoverPasswordCommand(RecoverPasswordRequestDTO request)
        {
            Request = request;
        }
    }
}
