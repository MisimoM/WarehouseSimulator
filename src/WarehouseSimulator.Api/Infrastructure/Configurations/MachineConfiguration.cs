using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseSimulator.Api.Domain.Machines;

namespace WarehouseSimulator.Api.Infrastructure.Configurations;

public class MachineConfiguration : IEntityTypeConfiguration<Machine>
{
    public void Configure(EntityTypeBuilder<Machine> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.LastBrokenAt);
        builder.Property(m => m.SimulatedLastBrokenAt);
        builder.Property(m => m.LastRepairedAt);
        builder.Property(m => m.SimulatedLastRepairedAt);

        builder.Property(m => m.TotalBreakdowns)
            .IsRequired()
            .HasDefaultValue(0);
    }
}
