using UsersMS.Domain.Entities;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Factories
{
    public interface IAuctioneerFactory
    {
        Auctioneer CreateAuctioneer(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null);

        Task<Auctioneer> GetAuctioneerById(UserId id);
    }
}
