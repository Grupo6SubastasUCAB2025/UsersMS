using Microsoft.EntityFrameworkCore;
using UsersMS.Domain.Entities;
using UsersMS.Domain.ValueObject;
using UsersMS.Infrastructure.Database;
using UsersMS.Domain.Repositories;

namespace UsersMS.Infrastructure.Repositories
{
    public class TechnicalSupportRepository : ITechnicalSupportRepository
    {
        private readonly UserDbContext _context;

        public TechnicalSupportRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<List<TechnicalSupport>> GetAllTechnicalSupportsAsync()
        {
            return await _context.TechnicalSupports.ToListAsync();
        }

        public async Task<TechnicalSupport> GetTechnicalSupportByIdAsync(Guid id)
        {
            var support = await _context.TechnicalSupports.FindAsync(new UserId(id));
            if (support == null)
            {
                throw new KeyNotFoundException($"TechnicalSupport with ID {id} not found.");
            }

            return support;
        }

        public async Task<List<TechnicalSupport>> GetTechnicalSupportsByNameAsync(string name)
        {
            return await _context.TechnicalSupports
                .Where(t => t.Name.Value.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task AddTechnicalSupportAsync(TechnicalSupport technicalSupport)
        {
            _context.TechnicalSupports.Add(technicalSupport);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTechnicalSupportAsync(TechnicalSupport technicalSupport)
        {
            var existing = await _context.TechnicalSupports.FindAsync(technicalSupport.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"TechnicalSupport with ID {technicalSupport.Id} not found.");
            }

            existing.ChangeName(technicalSupport.Name);
            existing.ChangePhone(technicalSupport.Phone);
            existing.ChangeCedula(technicalSupport.Cedula);

            await _context.SaveChangesAsync();
        }
    }
}
