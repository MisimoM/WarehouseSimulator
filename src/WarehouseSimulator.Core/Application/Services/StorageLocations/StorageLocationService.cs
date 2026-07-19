using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Core.Infrastructure.Persistence;

namespace WarehouseSimulator.Core.Application.Services.StorageLocations;

public class StorageLocationService( IDbContextFactory<ApplicationDbContext> dbContextFactory) : IStorageLocationService
{
    public async Task<List<StorageLocationView>> GetStorageLocationsAsync()
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.StorageLocations
            .Select(s => new StorageLocationView
            {
                Id = s.Id,
                Row = s.Row,
                Column = s.Column,
                Status = s.Status.ToString(),
                OrderNumber = s.Product != null ? s.Product.Order.OrderNumber : null
            })
            .ToListAsync();
    }
}
