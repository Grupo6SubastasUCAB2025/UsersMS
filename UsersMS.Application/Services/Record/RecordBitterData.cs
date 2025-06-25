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
    public class RecordBidderData : IRecordBidderData
    {
        private readonly IBidderFactory _bidderFactory;
        private readonly IBidderRepository _bidderRepository;
        private readonly IMapper _mapper;

        public RecordBidderData(IBidderFactory bidderFactory, IBidderRepository bidderRepository, IMapper mapper)
        {
            _bidderFactory = bidderFactory;
            _bidderRepository = bidderRepository;
            _mapper = mapper;
        }

        public async Task<RecordUserDataResponseDTO> Execute(RecordUserDataRequestDTO request)
        {
            var bidder = _bidderFactory.CreateBidder(
                new UserId(request.UserId),
                new UserName(request.Name),
                new UserEmail(request.UserEmail),
                new UserPhone(request.Phone),
            new UserCedula(request.Cedula)
            );
            await _bidderRepository.AddBidderAsync(bidder);

            return new RecordUserDataResponseDTO
            {
                Success = true,
                Message = "Bidder created successfully",
                UserEmail = request.UserEmail,
                UserId = request.UserId
            };
        }
    }
}
