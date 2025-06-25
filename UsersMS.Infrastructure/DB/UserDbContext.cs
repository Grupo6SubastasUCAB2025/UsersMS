using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UsersMS.Core.Infrastructure.DB;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.Database.Configuration;

namespace UsersMS.Infrastructure.Database
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbContext DbContext => this;

        public DbSet<Administrator> Administrators { get; set; } = null!;
        public DbSet<Auctioneer> Auctioneers { get; set; } = null!;
        public DbSet<Bidder> Bidders { get; set; } = null!;
        public DbSet<TechnicalSupport> TechnicalSupports { get; set; } = null!;

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public void ChangeEntityState<TEntity>(TEntity entity, EntityState state) where TEntity : class
        {
            if (entity != null)
            {
                Entry(entity).State = state;
            }
        }

        public async Task<bool> SaveEfContextChanges(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(cancellationToken) >= 0;
        }

        public async Task<bool> SaveEfContextChanges(string user, CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(cancellationToken) >= 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AdministratorConfiguration());
            modelBuilder.ApplyConfiguration(new AuctioneerConfiguration());
            modelBuilder.ApplyConfiguration(new BidderConfiguration());
            modelBuilder.ApplyConfiguration(new TechnicalSupportConfiguration());

            modelBuilder.Entity<Administrator>().HasKey(e => e.Id);
            modelBuilder.Entity<Auctioneer>().HasKey(e => e.Id);
            modelBuilder.Entity<Bidder>().HasKey(e => e.Id);
            modelBuilder.Entity<TechnicalSupport>().HasKey(e => e.Id);
        }
    }
}
