using UsersMS.Domain.Entities;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Factories
{
    public interface IAdministratorFactory
    {
        Administrator CreateAdministrator(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null);

        Task<Administrator> GetAdministratorById(UserId id);
    }
}
