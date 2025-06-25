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
    public class UpdateRecordAdministratorData : IUpdateRecordAdministratorData
    {
        private readonly IAdministratorFactory _administratorFactory;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IMapper _mapper;

        public UpdateRecordAdministratorData(IAdministratorFactory administratorFactory, IAdministratorRepository administratorRepository, IMapper mapper)
        {
            _administratorFactory = administratorFactory;
            _administratorRepository = administratorRepository;
            _mapper = mapper;
        }

        public async Task<UpdateRecordUserDataResponseDTO> Execute(UpdateRecordUserDataRequestDTO request)
        {
            var administrator = await _administratorFactory.GetAdministratorById(new UserId(request.UserId));
            ApplyChanges(administrator, request);

            await _administratorRepository.UpdateAdministratorAsync(administrator);

            return new UpdateRecordUserDataResponseDTO
            {
                Success = true,
                Message = "Administrator updated successfully",
                UserEmail = request.UserEmail,
                UserId = request.UserId
            };
        }

        private void ApplyChanges(Administrator admin, UpdateRecordUserDataRequestDTO request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                admin.ChangeName(new UserName(request.Name));
                admin.AddDomainEvent(new UserNameChangedEvent(admin.Id, new UserName(request.Name)));
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                admin.ChangePhone(new UserPhone(request.Phone));
                admin.AddDomainEvent(new UserPhoneChangedEvent(admin.Id, new UserPhone(request.Phone)));
            }

            if (!string.IsNullOrWhiteSpace(request.Cedula))
            {
                admin.ChangeCedula(new UserCedula(request.Cedula));
                admin.AddDomainEvent(new UserCedulaChangedEvent(admin.Id, new UserCedula(request.Cedula)));
            }
        }
    }
}
