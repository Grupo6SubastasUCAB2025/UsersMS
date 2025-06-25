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
    public class UpdateRecordAuctioneerData : IUpdateRecordAuctioneerData
    {
        private readonly IAuctioneerFactory _auctioneerFactory;
        private readonly IAuctioneerRepository _auctioneerRepository;
        private readonly IMapper _mapper;

        public UpdateRecordAuctioneerData(
            IAuctioneerFactory auctioneerFactory,
            IAuctioneerRepository auctioneerRepository,
            IMapper mapper)
        {
            _auctioneerFactory = auctioneerFactory;
            _auctioneerRepository = auctioneerRepository;
            _mapper = mapper;
        }

        public async Task<UpdateRecordUserDataResponseDTO> Execute(UpdateRecordUserDataRequestDTO request)
        {
            var auctioneer = await _auctioneerFactory.GetAuctioneerById(new UserId(request.UserId));
            ApplyChanges(auctioneer, request);

            await _auctioneerRepository.UpdateAuctioneerAsync(auctioneer);

            return new UpdateRecordUserDataResponseDTO
            {
                Success = true,
                Message = "Auctioneer updated successfully",
                UserEmail = request.UserEmail,
                UserId = request.UserId
            };
        }

        private void ApplyChanges(Auctioneer auctioneer, UpdateRecordUserDataRequestDTO request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                auctioneer.ChangeName(new UserName(request.Name));
                auctioneer.AddDomainEvent(new UserNameChangedEvent(auctioneer.Id, new UserName(request.Name)));
            }

            if (!string.IsNullOrWhiteSpace(request.Phone))
            {
                auctioneer.ChangePhone(new UserPhone(request.Phone));
                auctioneer.AddDomainEvent(new UserPhoneChangedEvent(auctioneer.Id, new UserPhone(request.Phone)));
            }

            if (!string.IsNullOrWhiteSpace(request.Cedula))
            {
                auctioneer.ChangeCedula(new UserCedula(request.Cedula));
                auctioneer.AddDomainEvent(new UserCedulaChangedEvent(auctioneer.Id, new UserCedula(request.Cedula)));
            }
        }
    }
}
