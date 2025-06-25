using MediatR;
using UsersMS.Infrastructure.DTOs.Update;
using UsersMS.Infrastructure.DTOs.UpdateUser;

namespace UsersMS.Application.Commands.UpdateUser
{
    public class UpdateRecordUserDataCommand : IRequest<UpdateRecordUserDataResponseDTO>
    {
        public UpdateRecordUserDataRequestDTO UpdateRecordUserDataRequestDTO { get; }

        public UpdateRecordUserDataCommand(UpdateRecordUserDataRequestDTO updateRecordUserDataRequestDTO)
        {
            UpdateRecordUserDataRequestDTO = updateRecordUserDataRequestDTO
                ?? throw new ArgumentNullException(nameof(updateRecordUserDataRequestDTO));
        }
    }
}
