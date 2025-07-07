using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UsersMS.Auth.Domain.Entities;

namespace UsersMS.Auth.Infrastructure.DB.Configuration
{
    public class NewBidderConfiguration : IEntityTypeConfiguration<NewBidder>
    {
        public void Configure(EntityTypeBuilder<NewBidder> builder)
        {
            builder.ToTable("BiddersRegistered");

            builder.HasKey(b => b.BidderId);

            builder.Property(b => b.BidderId)
                .IsRequired();
        }
    }
}
