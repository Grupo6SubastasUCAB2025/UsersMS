using UsersMS.Domain.ValueObject;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.DTOs.RecordUserData;
using UsersMS.Infrastructure.DTOs.Record;

namespace UsersMS.Application.Services.Record
{
    public class RecordUserDataService : IService<RecordUserDataRequestDTO, RecordUserDataResponseDTO>
    {
        private readonly IRecordAdministratorData _recordAdministratorData;
        private readonly IRecordBidderData _recordBidderData;
        private readonly IRecordAuctioneerData _recordAuctioneerData;
        private readonly IRecordTechnicalSupportData _recordTechnicalSupportData;

        public RecordUserDataService(
            IRecordAdministratorData recordAdministratorData,
            IRecordBidderData recordBidderData,
            IRecordAuctioneerData recordAuctioneerData,
            IRecordTechnicalSupportData recordTechnicalSupportData)
        {
            _recordAdministratorData = recordAdministratorData;
            _recordBidderData = recordBidderData;
            _recordAuctioneerData = recordAuctioneerData;
            _recordTechnicalSupportData = recordTechnicalSupportData;
        }

        public async Task<RecordUserDataResponseDTO> Execute(RecordUserDataRequestDTO request)
        {
            if (!Enum.TryParse(request.Role, out UserRole userRole))
            {
                return new RecordUserDataResponseDTO
                {
                    Success = false,
                    Message = "Invalid user role",
                    UserEmail = request.UserEmail,
                    UserId = Guid.Empty
                };
            }

            try
            {
                return userRole switch
                {
                    UserRole.Administrator => await _recordAdministratorData.Execute(request),
                    UserRole.Bidder => await _recordBidderData.Execute(request),
                    UserRole.Auctioneer => await _recordAuctioneerData.Execute(request),
                    UserRole.TechnicalSupport => await _recordTechnicalSupportData.Execute(request),
                    _ => throw new InvalidOperationException("Invalid user role")
                };
            }
            catch (Exception ex)
            {
                return new RecordUserDataResponseDTO
                {
                    Success = false,
                    Message = ex.Message + " " + ex.StackTrace,
                    UserEmail = request.UserEmail,
                    UserId = Guid.Empty
                };
            }
        }
    }
}
