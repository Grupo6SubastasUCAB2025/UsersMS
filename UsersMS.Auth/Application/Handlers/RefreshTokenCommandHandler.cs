using MediatR;
using UsersMS.Auth.Application.Commands;
using UsersMS.Auth.Infrastructure.DTOs.RefreshToken;
using UsersMS.Core.Application;

namespace UsersMS.Application.Handlers.Auth
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDTO>
    {
        private readonly IService<RefreshTokenRequestDTO, RefreshTokenResponseDTO> _refreshTokenService;

        public RefreshTokenCommandHandler(IService<RefreshTokenRequestDTO, RefreshTokenResponseDTO> refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<RefreshTokenResponseDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _refreshTokenService.Execute(request.RefreshTokenRequestDTO);
        }
    }
}
