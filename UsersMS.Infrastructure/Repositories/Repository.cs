using Microsoft.EntityFrameworkCore;

namespace UsersMS.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //protected readonly AuthDbContext _context;
        private readonly DbSet<T> _dbSet;

        /*public Repository(AuthDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }*/

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            //await _context.SaveChangesAsync();
        }

        public async Task<T?> GetById(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}
