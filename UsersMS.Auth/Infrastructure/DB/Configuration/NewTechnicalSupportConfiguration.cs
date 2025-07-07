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
    public class NewTechnicalSupportConfiguration : IEntityTypeConfiguration<NewTechnicalSupport>
    {
        public void Configure(EntityTypeBuilder<NewTechnicalSupport> builder)
        {
            builder.ToTable("TechnicalSupportRegistered");

            builder.HasKey(t => t.TechnicalSupportId);

            builder.Property(t => t.TechnicalSupportId)
                .IsRequired();
        }
    }
}
