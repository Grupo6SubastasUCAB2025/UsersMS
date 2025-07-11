using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Auth.Application.Commands;
using UsersMS.Auth.Infrastructure.DTOs.ChangePassword;
using UsersMS.Core.Application;

namespace UsersMS.Auth.Application.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResponseDTO>
    {
        private readonly IService<ChangePasswordRequestDTO, ChangePasswordResponseDTO> _changePasswordService;

        public ChangePasswordCommandHandler(IService<ChangePasswordRequestDTO, ChangePasswordResponseDTO> changePasswordService)
        {
            _changePasswordService = changePasswordService;
        }

        public async Task<ChangePasswordResponseDTO> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await _changePasswordService.Execute(request.ChangePasswordRequest);
        }
    }
}
