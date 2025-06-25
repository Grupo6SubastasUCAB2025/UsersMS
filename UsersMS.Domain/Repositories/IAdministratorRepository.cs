using UsersMS.Domain.Entities;

namespace UsersMS.Domain.Repositories
{
    public interface IAdministratorRepository
    {
        Task<List<Administrator>> GetAllAsync();
        Task<Administrator> GetByIdAsync(Guid id);
        Task<List<Administrator>> GetByNameAsync(string name);
        Task AddAdministratorAsync(Administrator administrator);
        Task UpdateAdministratorAsync(Administrator administrator);
    }
}
