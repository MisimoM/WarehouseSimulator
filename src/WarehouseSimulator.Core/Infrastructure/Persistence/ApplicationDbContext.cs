using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Core.Domain.Machines;
using WarehouseSimulator.Core.Domain.Notifications;
using WarehouseSimulator.Core.Domain.Orders;
using WarehouseSimulator.Core.Domain.Products;
using WarehouseSimulator.Core.Domain.StorageLocations;

namespace WarehouseSimulator.Core.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<StorageLocation> StorageLocations { get; set; }
    public DbSet<Machine> Machines { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}


