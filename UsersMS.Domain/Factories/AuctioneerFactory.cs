using UsersMS.Domain.Entities;
using UsersMS.Domain.Repositories;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Factories
{
    public class AuctioneerFactory : IAuctioneerFactory
    {
        private readonly IAuctioneerRepository _auctioneerRepository;

        public AuctioneerFactory(IAuctioneerRepository auctioneerRepository)
        {
            _auctioneerRepository = auctioneerRepository;
        }

        public Auctioneer CreateAuctioneer(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null)
        {
            return new Auctioneer(id, name, email, phone, cedula, isVerified, token);
        }

        public async Task<Auctioneer> GetAuctioneerById(UserId id)
        {
            return await _auctioneerRepository.GetByIdAsync(id.Id);
        }
    }
}
