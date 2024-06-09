using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mottu.Domain.Entities;

namespace Mottu.Infrastructure.Configurations
{
    public class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                .ValueGeneratedOnAdd();
            builder.HasIndex(m => m.LicensePlate).IsUnique();
            builder.Property(m => m.Year).IsRequired();
            builder.Property(m => m.Model).IsRequired();
            builder.Property(m => m.LicensePlate).IsRequired();
        }
    }
}
