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
    public class UpdateRecordBidderData : IUpdateRecordBidderData
    {
        private readonly IBidderFactory _bidderFactory;
        private readonly IBidderRepository _bidderRepository;
        private readonly IMapper _mapper;

        public UpdateRecordBidderData(
            IBidderFactory bidderFactory,
            IBidderRepository bidderRepository,
            IMapper mapper)
        {
            _bidderFactory = bidderFactory;
            _bidderRepository = bidderRepository;
            _mapper = mapper;
        }

        public async Task<UpdateRecordUserDataResponseDTO> Execute(UpdateRecordUserDataRequestDTO request)
        {
            var bidder = await _bidderFactory.GetBidderById(new UserId(request.UserId));
            ApplyChanges(bidder, request);

            await _bidderRepository.UpdateBidderAsync(bidder);

            return new UpdateRecordUserDataResponseDTO
            {
                Success = true,
                Message = "Bidder updated successfully",
                UserEmail = request.UserEmail,
                UserId = request.UserId
            };
        }

        private void ApplyChanges(Bidder bidder, UpdateRecordUserDataRequestDTO request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                bidder.ChangeName(new UserName(request.Name));
                bidder.AddDomainEvent(new UserNameChangedEvent(bidder.Id, new UserName(request.Name)));
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                bidder.ChangePhone(new UserPhone(request.Phone));
                bidder.AddDomainEvent(new UserPhoneChangedEvent(bidder.Id, new UserPhone(request.Phone)));
            }

            if (!string.IsNullOrWhiteSpace(request.Cedula))
            {
                bidder.ChangeCedula(new UserCedula(request.Cedula));
                bidder.AddDomainEvent(new UserCedulaChangedEvent(bidder.Id, new UserCedula(request.Cedula)));
            }
        }
    }
}
