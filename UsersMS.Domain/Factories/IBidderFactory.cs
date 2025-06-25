using UsersMS.Domain.Entities;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Factories
{
    public interface IBidderFactory
    {
        Bidder CreateBidder(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null);

        Task<Bidder> GetBidderById(UserId id);
    }
}
