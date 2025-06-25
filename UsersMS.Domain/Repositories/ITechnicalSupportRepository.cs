using UsersMS.Domain.Entities;

namespace UsersMS.Domain.Repositories
{
    public interface ITechnicalSupportRepository
    {
        Task<List<TechnicalSupport>> GetAllTechnicalSupportsAsync();
        Task<TechnicalSupport> GetTechnicalSupportByIdAsync(Guid id);
        Task<List<TechnicalSupport>> GetTechnicalSupportsByNameAsync(string name);
        Task AddTechnicalSupportAsync(TechnicalSupport technicalSupport);
        Task UpdateTechnicalSupportAsync(TechnicalSupport technicalSupport);
    }
}
