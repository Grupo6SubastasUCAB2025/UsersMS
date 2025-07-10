using MediatR;
using UsersMS.Auth.Application.Commands;
using UsersMS.Auth.Infrastructure.DTOs.Logout;
using UsersMS.Core.Application;

namespace UsersMS.Application.Handlers.Auth
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutResponseDTO>
    {
        private readonly IService<LogoutRequestDTO, LogoutResponseDTO> _logoutService;

        public LogoutCommandHandler(IService<LogoutRequestDTO, LogoutResponseDTO> logoutService)
        {
            _logoutService = logoutService;
        }

        public async Task<LogoutResponseDTO> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _logoutService.Execute(request.LogoutRequestDTO);
        }
    }
}
