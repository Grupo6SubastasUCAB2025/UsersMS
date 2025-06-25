using Microsoft.EntityFrameworkCore;
using UsersMS.Domain.Entities;
using UsersMS.Domain.Repositories;
using UsersMS.Domain.ValueObject;
using UsersMS.Infrastructure.Database;

namespace UsersMS.Infrastructure.Repositories
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly UserDbContext _context;

        public AdministratorRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<List<Administrator>> GetAllAsync()
        {
            return await _context.Administrators.ToListAsync();
        }

        public async Task<Administrator> GetByIdAsync(Guid id)
        {
            var entity = await _context.Administrators.FindAsync(new UserId(id));
            if (entity == null)
                throw new KeyNotFoundException($"Administrator with ID {id} not found.");

            return entity;
        }

        public async Task<List<Administrator>> GetByNameAsync(string name)
        {
            return await _context.Administrators
                .Where(a => a.Name.Value.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task AddAdministratorAsync(Administrator administrator)
        {
            _context.Administrators.Add(administrator);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAdministratorAsync(Administrator administrator)
        {
            var entity = await _context.Administrators.FindAsync(administrator.Id);
            if (entity == null)
                throw new KeyNotFoundException($"Administrator with ID {administrator.Id} not found.");

            entity.ChangeName(administrator.Name);
            entity.ChangePhone(administrator.Phone);
            entity.ChangeCedula(administrator.Cedula);

            await _context.SaveChangesAsync();
        }
    }
}
