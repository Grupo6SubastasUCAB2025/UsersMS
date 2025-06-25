using AutoMapper;
using UsersMS.Domain.Factories;
using UsersMS.Domain.Repositories;
using UsersMS.Domain.ValueObject;
using UsersMS.Infrastructure.DTOs;
using UsersMS.Infrastructure.DTOs.Record;
using UsersMS.Infrastructure.DTOs.RecordUserData;

namespace UsersMS.Application.Services.Record
{
    public class RecordAdministratorData : IRecordAdministratorData
    {
        private readonly IAdministratorFactory _administratorFactory;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IMapper _mapper;

        public RecordAdministratorData(
            IAdministratorFactory administratorFactory,
            IAdministratorRepository administratorRepository,
            IMapper mapper)
        {
            _administratorFactory = administratorFactory;
            _administratorRepository = administratorRepository;
            _mapper = mapper;
        }

        public async Task<RecordUserDataResponseDTO> Execute(RecordUserDataRequestDTO request)
        {
            var administrator = _administratorFactory.CreateAdministrator(
                new UserId(request.UserId),
                new UserName(request.Name),
                new UserEmail(request.UserEmail),
                new UserPhone(request.Phone),
                new UserCedula(request.Cedula)
            );

            await _administratorRepository.AddAdministratorAsync(administrator);

            return new RecordUserDataResponseDTO
            {
                Success = true,
                Message = "Administrator created successfully",
                UserEmail = request.UserEmail,
                UserId = request.UserId
            };
        }
    }
}
