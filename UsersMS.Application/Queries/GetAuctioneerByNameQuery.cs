using MediatR;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Queries.GetAuctioneerByName
{
    public class GetAuctioneerByNameQuery : IRequest<GetAuctioneerByNameResponseDTO>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public GetAuctioneerByNameQuery(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
