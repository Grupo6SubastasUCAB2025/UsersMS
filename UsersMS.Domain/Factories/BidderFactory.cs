using UsersMS.Domain.Entities;
using UsersMS.Domain.Repositories;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Factories
{
    public class BidderFactory : IBidderFactory
    {
        private readonly IBidderRepository _bidderRepository;

        public BidderFactory(IBidderRepository bidderRepository)
        {
            _bidderRepository = bidderRepository;
        }

        public Bidder CreateBidder(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null)
        {
            return new Bidder(id, name, email, phone, cedula, isVerified, token);
        }

        public async Task<Bidder> GetBidderById(UserId id)
        {
            return await _bidderRepository.GetBidderByIdAsync(id.Id);
        }
    }
}
