using AutoMapper;
using UsersMS.Domain.Entities;
using UsersMS.Domain.Factories;
using UsersMS.Domain.Repositories;
using UsersMS.Domain.ValueObject;
using UsersMS.Infrastructure.DTOs;
using UsersMS.Infrastructure.DTOs.Record;
using UsersMS.Infrastructure.DTOs.RecordUserData;
using UsersMS.Infrastructure.Repositories;

namespace UsersMS.Application.Services.Record
{
    public class RecordAuctioneerData : IRecordAuctioneerData
    {
        private readonly IAuctioneerFactory _auctioneerFactory;
        private readonly IAuctioneerRepository _auctioneerRepository;
        private readonly IMapper _mapper;

        public RecordAuctioneerData(IAuctioneerFactory auctioneerFactory, IAuctioneerRepository auctioneerRepository, IMapper mapper)
        {
            _auctioneerFactory = auctioneerFactory;
            _auctioneerRepository = auctioneerRepository;
            _mapper = mapper;
        }

        public async Task<RecordUserDataResponseDTO> Execute(RecordUserDataRequestDTO request)
        {
            var auctioneer = _auctioneerFactory.CreateAuctioneer(
                new UserId(request.UserId),
                new UserName(request.Name),
                new UserEmail(request.UserEmail),
                new UserPhone(request.Phone),
            new UserCedula(request.Cedula)
            );
            await _auctioneerRepository.AddAuctioneerAsync(auctioneer);

            return new RecordUserDataResponseDTO
            {
                Success = true,
                Message = "Auctioneer created successfully",
                UserEmail = request.UserEmail,
                UserId = request.UserId
            };
        }
    }
}
