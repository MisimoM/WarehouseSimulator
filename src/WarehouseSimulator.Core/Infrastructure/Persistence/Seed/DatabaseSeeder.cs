using WarehouseSimulator.Core.Domain.Machines;
using WarehouseSimulator.Core.Domain.StorageLocations;

namespace WarehouseSimulator.Core.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedMachinesAsync(context);
        await SeedStorageLocationsAsync(context);
    }

    private static async Task SeedMachinesAsync(ApplicationDbContext context)
    {
        if (context.Machines.Any())
            return;

        var machines = new List<Machine>
        {
            Machine.Create(MachineType.Production),
            Machine.Create(MachineType.Storage)
        };

        await context.Machines.AddRangeAsync(machines);
        await context.SaveChangesAsync();
    }

    private static async Task SeedStorageLocationsAsync(ApplicationDbContext context)
    {
        if (context.StorageLocations.Any())
            return;

        var locations = new List<StorageLocation>();
        var rows = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q" };

        foreach (var row in rows)
        {
            for (var column = 1; column <= 6; column++)
            {
                locations.Add(StorageLocation.Create(row, column));
            }
        }

        await context.StorageLocations.AddRangeAsync(locations);
        await context.SaveChangesAsync();
    }
}
