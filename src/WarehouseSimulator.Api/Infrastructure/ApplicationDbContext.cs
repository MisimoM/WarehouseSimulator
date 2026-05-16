using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Api.Domain.Orders;
using WarehouseSimulator.Api.Domain.Products;

namespace WarehouseSimulator.Api.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}


