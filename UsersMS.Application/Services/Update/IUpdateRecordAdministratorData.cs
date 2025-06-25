using UsersMS.Infrastructure.DTOs.Update;
using UsersMS.Infrastructure.DTOs.UpdateUser;

namespace UsersMS.Application.Services.Update
{
    public interface IUpdateRecordAdministratorData
    {
        Task<UpdateRecordUserDataResponseDTO> Execute(UpdateRecordUserDataRequestDTO request);
    }
}
