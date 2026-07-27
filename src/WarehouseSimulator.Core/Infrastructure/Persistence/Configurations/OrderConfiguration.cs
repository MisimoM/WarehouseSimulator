using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseSimulator.Core.Domain.Orders;

namespace WarehouseSimulator.Core.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedNever();

        builder.Property(o => o.OrderNumber).ValueGeneratedOnAdd();

        builder.Property(o => o.Priority)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(o => o.Region)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(o => o.DeliveryDeadline).IsRequired();
        builder.Property(o => o.DeliveredAt);

        builder.Property(o => o.CreatedAt).IsRequired();

        builder.HasOne(o => o.Truck)
            .WithMany()
            .HasForeignKey(o => o.TruckId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}