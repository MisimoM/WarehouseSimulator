using Microsoft.AspNetCore.SignalR;
using WarehouseSimulator.Core.Domain.Machines;
using WarehouseSimulator.Core.Domain.Machines.Events;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.SignalR;
using WarehouseSimulator.Core.SignalR.Messages;

namespace WarehouseSimulator.Core.Application.EventHandlers.Machines;

public class MachineRepairStartedEventHandler(IHubContext<WarehouseHub> hub) : IEventHandler<MachineRepairStartedEvent>
{
    public async Task Handle(MachineRepairStartedEvent @event)
    {
        await hub.Clients.All.SendAsync("MachineStatusUpdated", new MachineStatusUpdate(
            @event.MachineId,
            @event.MachineType.ToString(),
            MachineStatus.Repairing.ToString(),
            null
        ));
    }
}
