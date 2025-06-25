using AutoMapper;
using UsersMS.Domain.Events;
using UsersMS.Domain.Repositories;
using UsersMS.Domain.ValueObject;
using UsersMS.Domain.Factories;
using UsersMS.Domain.Entities;
using UsersMS.Application.Services.Update;
using UsersMS.Infrastructure.DTOs.Update;
using UsersMS.Infrastructure.DTOs.UpdateUser;

namespace UsersMS.Application.Services.UpdateUser
{
    public class UpdateRecordTechnicalSupportData : IUpdateRecordTechnicalSupportData
    {
        private readonly ITechnicalSupportFactory _technicalSupportFactory;
        private readonly ITechnicalSupportRepository _technicalSupportRepository;
        private readonly IMapper _mapper;

        public UpdateRecordTechnicalSupportData(
            ITechnicalSupportFactory technicalSupportFactory,
            ITechnicalSupportRepository technicalSupportRepository,
            IMapper mapper)
        {
            _technicalSupportFactory = technicalSupportFactory;
            _technicalSupportRepository = technicalSupportRepository;
            _mapper = mapper;
        }

        public async Task<UpdateRecordUserDataResponseDTO> Execute(UpdateRecordUserDataRequestDTO request)
        {
            var tech = await _technicalSupportFactory.GetTechnicalSupportById(new UserId(request.UserId));
            ApplyChanges(tech, request);

            await _technicalSupportRepository.UpdateTechnicalSupportAsync(tech);

            return new UpdateRecordUserDataResponseDTO
            {
                Success = true,
                Message = "Technical support updated successfully",
                UserEmail = request.UserEmail,
                UserId = request.UserId
            };
        }

        private void ApplyChanges(TechnicalSupport tech, UpdateRecordUserDataRequestDTO request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                tech.ChangeName(new UserName(request.Name));
                tech.AddDomainEvent(new UserNameChangedEvent(tech.Id, new UserName(request.Name)));
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                tech.ChangePhone(new UserPhone(request.Phone));
                tech.AddDomainEvent(new UserPhoneChangedEvent(tech.Id, new UserPhone(request.Phone)));
            }

            if (!string.IsNullOrWhiteSpace(request.Cedula))
            {
                tech.ChangeCedula(new UserCedula(request.Cedula));
                tech.AddDomainEvent(new UserCedulaChangedEvent(tech.Id, new UserCedula(request.Cedula)));
            }
        }
    }
}
