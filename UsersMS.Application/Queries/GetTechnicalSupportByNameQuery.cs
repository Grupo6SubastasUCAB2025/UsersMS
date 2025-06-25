using MediatR;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Queries.GetTechnicalSupportByName
{
    public class GetTechnicalSupportByNameQuery : IRequest<GetTechnicalSupportByNameResponseDTO>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public GetTechnicalSupportByNameQuery(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
