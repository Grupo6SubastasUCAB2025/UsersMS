using AutoMapper;
using MediatR;
using UsersMS.Application.Queries.GetTechnicalSupportById;
using UsersMS.Domain.Repositories;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Handlers.TechnicalSupport.GetTechnicalSupportById
{
    public class GetTechnicalSupportByIdQueryHandler : IRequestHandler<GetTechnicalSupportByIdQuery, GetTechnicalSupportByIdResponseDTO>
    {
        private readonly ITechnicalSupportRepository _repository;
        private readonly IMapper _mapper;

        public GetTechnicalSupportByIdQueryHandler(ITechnicalSupportRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetTechnicalSupportByIdResponseDTO> Handle(GetTechnicalSupportByIdQuery request, CancellationToken cancellationToken)
        {
            var technicalSupport = await _repository.GetTechnicalSupportByIdAsync(request.Id);
            var dto = _mapper.Map<TechnicalSupportDTO>(technicalSupport);
            return new GetTechnicalSupportByIdResponseDTO { TechnicalSupport = dto };
        }
    }
}
