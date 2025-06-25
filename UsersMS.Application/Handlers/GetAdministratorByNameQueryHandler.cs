using AutoMapper;
using MediatR;
using UsersMS.Application.Queries.GetAdministratorByName;
using UsersMS.Domain.Repositories;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Handlers.Administrator.GetAdministratorByName
{
    public class GetAdministratorByNameQueryHandler : IRequestHandler<GetAdministratorByNameQuery, GetAdministratorByNameResponseDTO>
    {
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IMapper _mapper;

        public GetAdministratorByNameQueryHandler(IAdministratorRepository administratorRepository, IMapper mapper)
        {
            _administratorRepository = administratorRepository;
            _mapper = mapper;
        }

        public async Task<GetAdministratorByNameResponseDTO> Handle(GetAdministratorByNameQuery request, CancellationToken cancellationToken)
        {
            var administrators = await _administratorRepository.GetByNameAsync(request.Name);
            var dtoList = _mapper.Map<List<AdministratorDTO>>(administrators);

            return new GetAdministratorByNameResponseDTO { Administrators = dtoList };
        }
    }
}
