using UsersMS.Infrastructure.DTOs.Update;
using UsersMS.Infrastructure.DTOs.UpdateUser;

namespace UsersMS.Application.Services.Update
{
    public interface IUpdateRecordAuctioneerData
    {
        Task<UpdateRecordUserDataResponseDTO> Execute(UpdateRecordUserDataRequestDTO request);
    }

}
