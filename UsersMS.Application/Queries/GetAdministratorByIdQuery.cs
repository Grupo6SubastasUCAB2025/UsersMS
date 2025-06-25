using MediatR;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Queries.GetAdministratorById
{
    public class GetAdministratorByIdQuery : IRequest<GetAdministratorByIdResponseDTO>
    {
        public Guid RequesterId { get; set; }
        public Guid Id { get; set; }

        public GetAdministratorByIdQuery(Guid requesterId, Guid id)
        {
            RequesterId = requesterId;
            Id = id;
        }
    }
}
