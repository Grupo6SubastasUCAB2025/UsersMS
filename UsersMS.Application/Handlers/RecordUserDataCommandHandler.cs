using MediatR;
using UsersMS.Application.Commands.RecordUserData;
using UsersMS.Application.Services;
using UsersMS.Core.Application;
using UsersMS.Infrastructure.DTOs;
using UsersMS.Infrastructure.DTOs.Record;
using UsersMS.Infrastructure.DTOs.RecordUserData;

namespace UsersMS.Application.Handlers.User.RecordUserData
{
    public class RecordUserDataCommandHandler : IRequestHandler<RecordUserDataCommand, RecordUserDataResponseDTO>
    {
        private readonly IService<RecordUserDataRequestDTO, RecordUserDataResponseDTO> _recordUserDataService;

        public RecordUserDataCommandHandler(IService<RecordUserDataRequestDTO, RecordUserDataResponseDTO> recordUserDataService)
        {
            _recordUserDataService = recordUserDataService;
        }

        public async Task<RecordUserDataResponseDTO> Handle(RecordUserDataCommand request, CancellationToken cancellationToken)
        {
            return await _recordUserDataService.Execute(request.RecordUserDataRequestDTO);
        }
    }
}
