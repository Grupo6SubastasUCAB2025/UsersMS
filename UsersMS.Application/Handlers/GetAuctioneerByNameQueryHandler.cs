using AutoMapper;
using MediatR;
using UsersMS.Application.Queries.GetAuctioneerByName;
using UsersMS.Domain.Repositories;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Handlers.Auctioneer.GetAuctioneerByName
{
    public class GetAuctioneerByNameQueryHandler : IRequestHandler<GetAuctioneerByNameQuery, GetAuctioneerByNameResponseDTO>
    {
        private readonly IAuctioneerRepository _auctioneerRepository;
        private readonly IMapper _mapper;

        public GetAuctioneerByNameQueryHandler(IAuctioneerRepository auctioneerRepository, IMapper mapper)
        {
            _auctioneerRepository = auctioneerRepository;
            _mapper = mapper;
        }

        public async Task<GetAuctioneerByNameResponseDTO> Handle(GetAuctioneerByNameQuery request, CancellationToken cancellationToken)
        {
            var auctioneers = await _auctioneerRepository.GetByNameAsync(request.Name);
            var dtoList = _mapper.Map<List<AuctioneerDTO>>(auctioneers);

            return new GetAuctioneerByNameResponseDTO { Auctioneers = dtoList };
        }
    }
}
