using AutoMapper;
using MediatR;
using UsersMS.Application.Queries.GetBidderById;
using UsersMS.Domain.Repositories;
using UsersMS.Infrastructure.DTOs;

namespace UsersMS.Application.Handlers.Bidder.GetBidderById
{
    public class GetBidderByIdQueryHandler : IRequestHandler<GetBidderByIdQuery, GetBidderByIdResponseDTO>
    {
        private readonly IBidderRepository _bidderRepository;
        private readonly IMapper _mapper;

        public GetBidderByIdQueryHandler(IBidderRepository bidderRepository, IMapper mapper)
        {
            _bidderRepository = bidderRepository;
            _mapper = mapper;
        }

        public async Task<GetBidderByIdResponseDTO> Handle(GetBidderByIdQuery request, CancellationToken cancellationToken)
        {
            var bidder = await _bidderRepository.GetBidderByIdAsync(request.BidderId);
            var dto = _mapper.Map<BidderDTO>(bidder);
            return new GetBidderByIdResponseDTO { Bidder = dto };
        }
    }
}
