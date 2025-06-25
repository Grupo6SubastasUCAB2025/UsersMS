using MediatR;
using UsersMS.Infrastructure.DTOs.Record;
using UsersMS.Infrastructure.DTOs.RecordUserData;

namespace UsersMS.Application.Commands.RecordUserData
{
    public class RecordUserDataCommand : IRequest<RecordUserDataResponseDTO>
    {
        public RecordUserDataRequestDTO RecordUserDataRequestDTO { get; }

        public RecordUserDataCommand(RecordUserDataRequestDTO recordUserDataRequestDTO)
        {
            RecordUserDataRequestDTO = recordUserDataRequestDTO ??
                throw new ArgumentNullException(nameof(recordUserDataRequestDTO));
        }
    }
}
