using MediatR;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Queries.GetAdministratorByName
{
    public class GetAdministratorByNameQuery : IRequest<GetAdministratorByNameResponseDTO>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }

        public GetAdministratorByNameQuery(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
