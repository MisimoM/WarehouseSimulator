using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseSimulator.Api.Domain.StorageLocations;

namespace WarehouseSimulator.Api.Infrastructure.Configurations;

public class StorageLocationConfiguration : IEntityTypeConfiguration<StorageLocation>
{
    public void Configure(EntityTypeBuilder<StorageLocation> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.Property(s => s.Row).IsRequired();

        builder.Property(s => s.Column).IsRequired();

        builder.Property(s => s.LocationVersion).IsRowVersion();

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(s => s.Product)
            .WithOne()
            .HasForeignKey<StorageLocation>(s => s.ProductId);
    }
}
