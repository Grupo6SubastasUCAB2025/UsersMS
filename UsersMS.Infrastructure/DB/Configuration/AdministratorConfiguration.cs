using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UsersMS.Domain.Entities;
using UsersMS.Domain.ValueObject;

namespace UsersMS.Infrastructure.Database.Configuration
{
    public class AdministratorConfiguration : IEntityTypeConfiguration<Administrator>
    {
        public void Configure(EntityTypeBuilder<Administrator> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .HasConversion(id => id.Value.ToString(), str => new UserId(Guid.Parse(str)))
                .IsRequired();

            builder.Property(a => a.Name)
                .HasConversion(name => name.Value, str => new UserName(str))
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Email)
                .HasConversion(email => email.Value, str => new UserEmail(str))
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Phone)
                .HasConversion(phone => phone.Value, str => new UserPhone(str))
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(a => a.Cedula)
                .HasConversion(cedula => cedula.Value, str => new UserCedula(str))
                .IsRequired()
                .HasMaxLength(8);
        }
    }
}
