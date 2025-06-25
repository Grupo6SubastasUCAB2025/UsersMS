using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace UsersMS.Core.Infrastructure.DB
{
    public interface IUserDbContext
    {
        DbContext DbContext { get; }

        IDbContextTransaction BeginTransaction();

        void ChangeEntityState<TEntity>(TEntity entity, EntityState state) where TEntity : class;

        Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default);
    }
}
