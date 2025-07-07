using MediatR;
using UsersMS.Application.DTOs.Auth;
using UsersMS.Auth.Application.Commands;
using UsersMS.Auth.Infrastructure.DTOs.Login;
using UsersMS.Core.Application;

namespace UsersMS.Auth.Application.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDTO>
    {
        private readonly IService<LoginRequestDTO, LoginResponseDTO> _loginService;

        public LoginCommandHandler(IService<LoginRequestDTO, LoginResponseDTO> loginService)
        {
            _loginService = loginService;
        }

        public async Task<LoginResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _loginService.Execute(request.LoginRequestDTO);
        }
    }
}
