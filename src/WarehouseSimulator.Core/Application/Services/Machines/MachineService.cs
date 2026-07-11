using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Core.Domain.Machines;
using WarehouseSimulator.Core.Infrastructure.Persistence;

namespace WarehouseSimulator.Core.Application.Services.Machines;

public class MachineService(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IMachineService
{
    public async Task<List<Machine>> GetMachinesAsync()
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Machines.ToListAsync();
    }

    public async Task RepairMachineAsync(int machineId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var machine = await context.Machines.FindAsync(machineId);
        machine!.StartRepair();
        await context.SaveChangesAsync();
    }
}
