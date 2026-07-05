using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Api.Domain.Products;
using WarehouseSimulator.Api.Infrastructure.Persistence;

namespace WarehouseSimulator.Api.Infrastructure.Belt;

public class BeltRestoreService(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    BeltChannel belt,
    ILogger<BeltRestoreService> logger)
{
    public async Task RestoreAsync()
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        var productsOnBelt = await context.Products
            .Where(p => p.Status == ProductStatus.OnBelt)
            .ToListAsync();

        if (productsOnBelt.Count == 0)
        {
            logger.LogInformation("No products to restore on belt");
            return;
        }

        foreach (var product in productsOnBelt)
        {
            await belt.Writer.WriteAsync(product);
        }

        logger.LogInformation("Restored {Count} products to belt", productsOnBelt.Count);
    }
}