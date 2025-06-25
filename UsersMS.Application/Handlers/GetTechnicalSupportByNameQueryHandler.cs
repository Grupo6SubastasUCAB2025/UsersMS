using AutoMapper;
using MediatR;
using UsersMS.Application.Queries.GetTechnicalSupportByName;
using UsersMS.Domain.Repositories;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Handlers.TechnicalSupport.GetTechnicalSupportByName
{
    public class GetTechnicalSupportByNameQueryHandler : IRequestHandler<GetTechnicalSupportByNameQuery, GetTechnicalSupportByNameResponseDTO>
    {
        private readonly ITechnicalSupportRepository _technicalSupportRepository;
        private readonly IMapper _mapper;

        public GetTechnicalSupportByNameQueryHandler(ITechnicalSupportRepository technicalSupportRepository, IMapper mapper)
        {
            _technicalSupportRepository = technicalSupportRepository;
            _mapper = mapper;
        }

        public async Task<GetTechnicalSupportByNameResponseDTO> Handle(GetTechnicalSupportByNameQuery request, CancellationToken cancellationToken)
        {
            var technicalSupports = await _technicalSupportRepository.GetTechnicalSupportsByNameAsync(request.Name);
            var dtoList = _mapper.Map<List<TechnicalSupportDTO>>(technicalSupports);

            return new GetTechnicalSupportByNameResponseDTO { TechnicalSupports = dtoList };
        }
    }
}
