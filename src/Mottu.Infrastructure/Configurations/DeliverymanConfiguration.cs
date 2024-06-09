using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mottu.Domain.Entities;

namespace Mottu.Infrastructure.Configurations
{
    public class DeliverymanConfiguration : IEntityTypeConfiguration<Deliveryman>
    {
        public void Configure(EntityTypeBuilder<Deliveryman> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Cnpj).IsRequired().HasMaxLength(14);
            builder.Property(d => d.BirthDate).IsRequired();
            builder.Property(d => d.DriverLicenseNumber).IsRequired().HasMaxLength(11);
            builder.Property(d => d.LicenseType).IsRequired().HasMaxLength(3);
            builder.Property(d => d.LicenseImagePath).IsRequired();
            builder.HasOne(d => d.User)
                   .WithMany()
                   .HasForeignKey(d => d.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(d => d.Cnpj).IsUnique();
            builder.HasIndex(d => d.DriverLicenseNumber).IsUnique();
        }
    }
}
