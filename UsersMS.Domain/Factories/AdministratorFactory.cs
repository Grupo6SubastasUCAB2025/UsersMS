using UsersMS.Domain.Entities;
using UsersMS.Domain.Repositories;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Factories
{
    public class AdministratorFactory : IAdministratorFactory
    {
        private readonly IAdministratorRepository _administratorRepository;

        public AdministratorFactory(IAdministratorRepository administratorRepository)
        {
            _administratorRepository = administratorRepository;
        }

        public Administrator CreateAdministrator(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null)
        {
            return new Administrator(id, name, email, phone, cedula, isVerified, token);
        }

        public async Task<Administrator> GetAdministratorById(UserId id)
        {
            return await _administratorRepository.GetByIdAsync(id.Id);
        }

    }
}
