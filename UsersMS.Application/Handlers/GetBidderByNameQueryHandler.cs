using AutoMapper;
using MediatR;
using UsersMS.Application.Queries.GetBidderByName;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Handlers.Bidder.GetBidderByName
{
    public class GetBidderByNameQueryHandler : IRequestHandler<GetBidderByNameQuery, GetBidderByNameResponseDTO>
    {
        private readonly IBidderRepository _bidderRepository;
        private readonly IMapper _mapper;

        public GetBidderByNameQueryHandler(IBidderRepository bidderRepository, IMapper mapper)
        {
            _bidderRepository = bidderRepository;
            _mapper = mapper;
        }

        public async Task<GetBidderByNameResponseDTO> Handle(GetBidderByNameQuery request, CancellationToken cancellationToken)
        {
            var bidders = await _bidderRepository.GetBiddersByNameAsync(request.Name);
            var dtoList = _mapper.Map<List<BidderDTO>>(bidders);

            return new GetBidderByNameResponseDTO { Bidders = dtoList };
        }
    }
}
