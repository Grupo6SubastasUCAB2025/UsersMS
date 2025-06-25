using UsersMS.Domain.Entities;
using UsersMS.Domain.Repositories;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Domain.Factories
{
    public class TechnicalSupportFactory : ITechnicalSupportFactory
    {
        private readonly ITechnicalSupportRepository _technicalSupportRepository;

        public TechnicalSupportFactory(ITechnicalSupportRepository technicalSupportRepository)
        {
            _technicalSupportRepository = technicalSupportRepository;
        }

        public TechnicalSupport CreateTechnicalSupport(
            UserId id,
            UserName name,
            UserEmail email,
            UserPhone phone,
            UserCedula cedula,
            bool isVerified = false,
            UserToken? token = null)
        {
            return new TechnicalSupport(id, name, email, phone, cedula, isVerified, token);
        }

        public async Task<TechnicalSupport> GetTechnicalSupportById(UserId id)
        {
            return await _technicalSupportRepository.GetTechnicalSupportByIdAsync(id.Id);
        }
    }
}
