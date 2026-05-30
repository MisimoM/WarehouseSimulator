using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Api.Domain.Machines;
using WarehouseSimulator.Api.Domain.Notifications;
using WarehouseSimulator.Api.Domain.Orders;
using WarehouseSimulator.Api.Domain.Products;
using WarehouseSimulator.Api.Domain.StorageLocations;

namespace WarehouseSimulator.Api.Infrastructure.Persistence;

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


