using MediatR;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Queries.GetBidderById
{
    public class GetBidderByIdQuery : IRequest<GetBidderByIdResponseDTO>
    {
        public Guid UserId { get; set; }
        public Guid BidderId { get; set; }

        public GetBidderByIdQuery(Guid userId, Guid bidderId)
        {
            UserId = userId;
            BidderId = bidderId;
        }
    }
}
