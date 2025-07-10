using Microsoft.EntityFrameworkCore;
using UsersMS.Auth.Domain.Entities;
using UsersMS.Auth.Infrastructure.DB.Configuration;

namespace UsersMS.Infrastructure.Database
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<NewBidder> NewBidders { get; set; } = null!;
        public DbSet<NewAuctioneer> NewAuctioneers { get; set; } = null!;
        public DbSet<NewTechnicalSupport> NewTechnicalSupports { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new NewBidderConfiguration());
            modelBuilder.ApplyConfiguration(new NewAuctioneerConfiguration());
            modelBuilder.ApplyConfiguration(new NewTechnicalSupportConfiguration());

            modelBuilder.Entity<NewBidder>()
                .HasKey(b => b.BidderId);

            modelBuilder.Entity<NewAuctioneer>()
                .HasKey(a => a.AuctioneerId);

            modelBuilder.Entity<NewTechnicalSupport>()
                .HasKey(t => t.TechnicalSupportId);
        }
    }
}
