using Microsoft.AspNetCore.SignalR;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.Domain.StorageLocations.Events;
using WarehouseSimulator.Core.SignalR;
using WarehouseSimulator.Core.SignalR.Messages;

namespace WarehouseSimulator.Core.Application.EventHandlers.StorageLocations;

public class StorageLocationUpdatedEventHandler(IHubContext<WarehouseHub> hub) : IEventHandler<StorageLocationUpdatedEvent>
{
    public async Task Handle(StorageLocationUpdatedEvent @event)
    {
        await hub.Clients.All.SendAsync("StorageLocationUpdated", new StorageLocationUpdate(
            @event.LocationId,
            @event.Row,
            @event.Column,
            @event.Status.ToString(),
            @event.ProductId,
            @event.OrderNumber
        ));
    }
}
