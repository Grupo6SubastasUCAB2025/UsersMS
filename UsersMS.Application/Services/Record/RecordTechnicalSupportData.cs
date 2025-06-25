using AutoMapper;
using UsersMS.Domain.Factories;
using UsersMS.Domain.Repositories;
using UsersMS.Domain.ValueObject;
using UsersMS.Infrastructure.DTOs.Record;
using UsersMS.Infrastructure.DTOs.RecordUserData;

namespace UsersMS.Application.Services.Record
{
    public class RecordTechnicalSupportData : IRecordTechnicalSupportData
    {
        private readonly ITechnicalSupportFactory _technicalSupportFactory;
        private readonly ITechnicalSupportRepository _technicalSupportRepository;
        private readonly IMapper _mapper;

        public RecordTechnicalSupportData(ITechnicalSupportFactory technicalSupportFactory, ITechnicalSupportRepository technicalSupportRepository, IMapper mapper)
        {
            _technicalSupportFactory = technicalSupportFactory;
            _technicalSupportRepository = technicalSupportRepository;
            _mapper = mapper;
        }

        public async Task<RecordUserDataResponseDTO> Execute(RecordUserDataRequestDTO request)
        {
            var technicalSupport = _technicalSupportFactory.CreateTechnicalSupport(
                new UserId(request.UserId),
                new UserName(request.Name),
                new UserEmail(request.UserEmail),
                new UserPhone(request.Phone),
            new UserCedula(request.Cedula)
            );
            await _technicalSupportRepository.AddTechnicalSupportAsync(technicalSupport);

            return new RecordUserDataResponseDTO
            {
                Success = true,
                Message = "Technical Support created successfully",
                UserEmail = request.UserEmail,
                UserId = request.UserId
            };
        }
    }
}
