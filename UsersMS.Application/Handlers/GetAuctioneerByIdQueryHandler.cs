using AutoMapper;
using MediatR;
using UsersMS.Application.Queries.GetAuctioneerById;
using UsersMS.Domain.Repositories;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Handlers.Auctioneer.GetAuctioneerById
{
    public class GetAuctioneerByIdQueryHandler : IRequestHandler<GetAuctioneerByIdQuery, GetAuctioneerByIdResponseDTO>
    {
        private readonly IAuctioneerRepository _auctioneerRepository;
        private readonly IMapper _mapper;

        public GetAuctioneerByIdQueryHandler(IAuctioneerRepository auctioneerRepository, IMapper mapper)
        {
            _auctioneerRepository = auctioneerRepository;
            _mapper = mapper;
        }

        public async Task<GetAuctioneerByIdResponseDTO> Handle(GetAuctioneerByIdQuery request, CancellationToken cancellationToken)
        {
            var auctioneer = await _auctioneerRepository.GetByIdAsync(request.UserId);
            var dto = _mapper.Map<AuctioneerDTO>(auctioneer);
            return new GetAuctioneerByIdResponseDTO { Auctioneer = dto };
        }
    }
}
