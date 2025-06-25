using UsersMS.Application.Services.Update;
using UsersMS.Core.Application;
using UsersMS.Domain.ValueObject;
using UsersMS.Infrastructure.DTOs.Update;
using UsersMS.Infrastructure.DTOs.UpdateUser;

namespace UsersMS.Application.Services.UpdateUser
{
    public class UpdateRecordUserDataService : IService<UpdateRecordUserDataRequestDTO, UpdateRecordUserDataResponseDTO>
    {
        private readonly IUpdateRecordAdministratorData _administratorDataUpdater;
        private readonly IUpdateRecordAuctioneerData _auctioneerDataUpdater;
        private readonly IUpdateRecordBidderData _bidderDataUpdater;
        private readonly IUpdateRecordTechnicalSupportData _technicalSupportDataUpdater;

        public UpdateRecordUserDataService(
            IUpdateRecordAdministratorData administratorDataUpdater,
            IUpdateRecordAuctioneerData auctioneerDataUpdater,
            IUpdateRecordBidderData bidderDataUpdater,
            IUpdateRecordTechnicalSupportData technicalSupportDataUpdater)
        {
            _administratorDataUpdater = administratorDataUpdater;
            _auctioneerDataUpdater = auctioneerDataUpdater;
            _bidderDataUpdater = bidderDataUpdater;
            _technicalSupportDataUpdater = technicalSupportDataUpdater;
        }

        public async Task<UpdateRecordUserDataResponseDTO> Execute(UpdateRecordUserDataRequestDTO request)
        {
            if (!Enum.TryParse(request.Role, ignoreCase: true, out UserRole userRole))
            {
                return new UpdateRecordUserDataResponseDTO
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
                    UserRole.Administrator => await _administratorDataUpdater.Execute(request),
                    UserRole.Auctioneer => await _auctioneerDataUpdater.Execute(request),
                    UserRole.Bidder => await _bidderDataUpdater.Execute(request),
                    UserRole.TechnicalSupport => await _technicalSupportDataUpdater.Execute(request),
                    _ => throw new InvalidOperationException("Unsupported user role")
                };
            }
            catch (Exception ex)
            {
                return new UpdateRecordUserDataResponseDTO
                {
                    Success = false,
                    Message = ex.Message,
                    UserEmail = request.UserEmail,
                    UserId = Guid.Empty
                };
            }
        }
    }
}
