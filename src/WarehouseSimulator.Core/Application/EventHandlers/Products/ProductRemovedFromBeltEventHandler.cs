using Microsoft.AspNetCore.SignalR;
using WarehouseSimulator.Core.Domain.Products.Events;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.SignalR;
using WarehouseSimulator.Core.SignalR.Messages;

namespace WarehouseSimulator.Core.Application.EventHandlers.Products;

public class ProductRemovedFromBeltEventHandler(IHubContext<WarehouseHub> hub) : IEventHandler<ProductRemovedFromBeltEvent>
{
    public async Task Handle(ProductRemovedFromBeltEvent @event)
    {
        await hub.Clients.All.SendAsync("BeltUpdated", new BeltUpdate(
            @event.ProductId,
            null,
            null,
            false
        ));
    }
}
