using AutoMapper;
using MediatR;
using UsersMS.Application.Queries.GetAdministratorById;
using UsersMS.Domain.Repositories;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Handlers.Administrator.GetAdministratorById
{
    public class GetAdministratorByIdQueryHandler : IRequestHandler<GetAdministratorByIdQuery, GetAdministratorByIdResponseDTO>
    {
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IMapper _mapper;

        public GetAdministratorByIdQueryHandler(IAdministratorRepository administratorRepository, IMapper mapper)
        {
            _administratorRepository = administratorRepository;
            _mapper = mapper;
        }


        public async Task<GetAdministratorByIdResponseDTO> Handle(GetAdministratorByIdQuery request, CancellationToken cancellationToken)
        {
            var administrator = await _administratorRepository.GetByIdAsync(request.Id);
            var dto = _mapper.Map<AdministratorDTO>(administrator);
            return new GetAdministratorByIdResponseDTO { Administrator = dto };
        }
    }
}
