using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseSimulator.Api.Domain.Products;

namespace WarehouseSimulator.Api.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.OrderId).IsRequired();

        builder.Property(p => p.Priority)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.ProducedAt).IsRequired();
        builder.Property(p => p.SimulatedProducedAt).IsRequired();

        builder.Property(p => p.StoredAt);
        builder.Property(p => p.SimulatedStoredAt);

        builder.HasOne(p => p.Order)
            .WithOne()
            .HasForeignKey<Product>(p => p.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
