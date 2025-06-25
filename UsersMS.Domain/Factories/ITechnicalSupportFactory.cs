using UsersMS.Domain.Entities;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Factories
{
    public interface ITechnicalSupportFactory
    {
        TechnicalSupport CreateTechnicalSupport(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null);

        Task<TechnicalSupport> GetTechnicalSupportById(UserId id);
    }
}
