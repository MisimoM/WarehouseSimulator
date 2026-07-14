using Microsoft.AspNetCore.SignalR;
using WarehouseSimulator.Core.Domain.Machines;
using WarehouseSimulator.Core.Domain.Machines.Events;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.SignalR;
using WarehouseSimulator.Core.SignalR.Messages;

namespace WarehouseSimulator.Core.Application.EventHandlers.Machines;

public class MachineBreakdownHandler(IHubContext<WarehouseHub> hub) : IEventHandler<MachineBreakdownEvent>
{
    public async Task Handle(MachineBreakdownEvent @event)
    {
        await hub.Clients.All.SendAsync("MachineStatusUpdated", new MachineStatusUpdate(
            @event.MachineId,
            @event.MachineType.ToString(),
            MachineStatus.Broken.ToString(),
            @event.TotalBreakdowns
        ));
    }
}
