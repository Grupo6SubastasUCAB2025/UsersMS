using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Auth.Application.Commands;
using UsersMS.Auth.Infrastructure.DTOs.RecoverPassword;
using UsersMS.Core.Application;

namespace UsersMS.Auth.Application.Handlers
{
    public class RecoverPasswordCommandHandler : IRequestHandler<RecoverPasswordCommand, RecoverPasswordResponseDTO>
    {
        private readonly IService<RecoverPasswordRequestDTO, RecoverPasswordResponseDTO> _recoverPasswordService;

        public RecoverPasswordCommandHandler(IService<RecoverPasswordRequestDTO, RecoverPasswordResponseDTO> recoverPasswordService)
        {
            _recoverPasswordService = recoverPasswordService;
        }

        public async Task<RecoverPasswordResponseDTO> Handle(RecoverPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _recoverPasswordService.Execute(request.Request);
        }
    }
}
