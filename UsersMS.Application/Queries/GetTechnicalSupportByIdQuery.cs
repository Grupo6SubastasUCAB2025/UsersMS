using MediatR;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Queries.GetTechnicalSupportById
{
    public class GetTechnicalSupportByIdQuery : IRequest<GetTechnicalSupportByIdResponseDTO>
    {
        public Guid RequesterId { get; set; }
        public Guid Id { get; set; }

        public GetTechnicalSupportByIdQuery(Guid requesterId, Guid technicalSupportId)
        {
            RequesterId = requesterId;
            Id = technicalSupportId;
        }
    }
}
