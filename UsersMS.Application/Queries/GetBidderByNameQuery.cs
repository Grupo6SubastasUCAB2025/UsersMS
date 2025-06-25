using MediatR;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Queries.GetBidderByName
{
    public class GetBidderByNameQuery : IRequest<GetBidderByNameResponseDTO>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public GetBidderByNameQuery(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
