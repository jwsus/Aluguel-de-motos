
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mottu.Domain.Entities;

namespace Mottu.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.RideValue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            builder.Property(o => o.Situation)
                .IsRequired();
            builder.HasOne(o => o.Deliveryman)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DeliverymanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
