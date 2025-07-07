using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Auth.Domain.Entities;

namespace UsersMS.Auth.Infrastructure.DB.Configuration
{
    public class NewAuctioneerConfiguration : IEntityTypeConfiguration<NewAuctioneer>
    {
        public void Configure(EntityTypeBuilder<NewAuctioneer> builder)
        {
            builder.ToTable("AuctioneersRegistered");

            builder.HasKey(a => a.AuctioneerId);

            builder.Property(a => a.AuctioneerId)
                .IsRequired();
        }
    }
}
