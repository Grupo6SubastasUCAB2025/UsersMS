using MediatR;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Queries.GetAuctioneerById
{
    public class GetAuctioneerByIdQuery : IRequest<GetAuctioneerByIdResponseDTO>
    {
        public Guid RequesterId { get; set; }
        public Guid UserId { get; set; }

        public GetAuctioneerByIdQuery(Guid requesterId, Guid userId)
        {
            RequesterId = requesterId;
            UserId = userId;
        }
    }
}
