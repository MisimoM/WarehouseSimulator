using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseSimulator.Core.Domain.Notifications;

namespace WarehouseSimulator.Core.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).ValueGeneratedOnAdd();

        builder.Property(n => n.Message).IsRequired();

        builder.Property(n => n.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(n => n.Source).IsRequired();

        builder.Property(n => n.CreatedAt).IsRequired();
        builder.Property(n => n.SimulatedCreatedAt).IsRequired();

        builder.Property(n => n.IsRead).HasDefaultValue(false);
        builder.Property(n => n.IsResolved).HasDefaultValue(false);
    }
}
