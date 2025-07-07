using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Auth.Infrastructure.DTOs.RefreshToken;

namespace UsersMS.Auth.Application.Commands
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponseDTO>
    {
        public RefreshTokenRequestDTO RefreshTokenRequestDTO { get; set; }

        public RefreshTokenCommand(RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            RefreshTokenRequestDTO = refreshTokenRequestDTO;
        }
    }
}
