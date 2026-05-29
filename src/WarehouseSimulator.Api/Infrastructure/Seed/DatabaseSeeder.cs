using WarehouseSimulator.Api.Domain.Machines;
using WarehouseSimulator.Api.Domain.StorageLocations;

namespace WarehouseSimulator.Api.Infrastructure.Seed;

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
        var rows = new[] { "A", "B", "C", "D" };

        foreach (var row in rows)
        {
            for (var column = 1; column <= 10; column++)
            {
                locations.Add(StorageLocation.Create(row, column));
            }
        }

        await context.StorageLocations.AddRangeAsync(locations);
        await context.SaveChangesAsync();
    }
}
