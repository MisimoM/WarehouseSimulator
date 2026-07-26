using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WarehouseSimulator.Core.Domain.Machines.Events;
using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.Infrastructure.Persistence;

namespace WarehouseSimulator.Core.Application.Services.Machines;

public class MachineService(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    IEventBus eventBus,
    ISimulationClock simulationClock,
    ILogger<MachineService> logger) : IMachineService
{
    public async Task<List<MachineView>> GetMachinesAsync()
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Machines
            .Select(m => new MachineView
            {
                Id = m.Id,
                Type = m.Type,
                Status = m.Status,
                TotalBreakdowns = m.TotalBreakdowns
            })
            .ToListAsync();
    }

    public async Task RepairMachineAsync(int machineId)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        var machine = await context.Machines.FindAsync(machineId);

        machine!.StartRepair();
        await context.SaveChangesAsync();
        await eventBus.PublishAsync(new MachineRepairStartedEvent(
            machine.Id,
            machine.Type
        ));

        logger.LogInformation("Machine {MachineType} repair started", machine.Type);

        await simulationClock.Delay(TimeSpan.FromHours(1));

        machine.FinishRepair(simulationClock.GetCurrentSimulatedTime());
        await context.SaveChangesAsync();
        
        await eventBus.PublishAsync(new MachineRepairedEvent(
            machine.Id,
            machine.Type
        ));

        logger.LogInformation("Machine {MachineType} repair finished", machine.Type);
    }
}
