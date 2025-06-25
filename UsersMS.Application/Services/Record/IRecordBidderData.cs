using UsersMS.Infrastructure.DTOs.Record;
using UsersMS.Infrastructure.DTOs.RecordUserData;

namespace UsersMS.Application.Services.Record
{
    public interface IRecordBidderData
    {
        Task<RecordUserDataResponseDTO> Execute(RecordUserDataRequestDTO request);
    }
}
