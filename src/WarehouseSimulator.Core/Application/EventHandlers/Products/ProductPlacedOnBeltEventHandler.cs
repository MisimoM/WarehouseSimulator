using Microsoft.AspNetCore.SignalR;
using WarehouseSimulator.Core.Domain.Products.Events;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.SignalR;
using WarehouseSimulator.Core.SignalR.Messages;

namespace WarehouseSimulator.Core.Application.EventHandlers.Products;

public class ProductPlacedOnBeltEventHandler(IHubContext<WarehouseHub> hub) : IEventHandler<ProductPlacedOnBeltEvent>
{
    public async Task Handle(ProductPlacedOnBeltEvent @event)
    {
        await hub.Clients.All.SendAsync("BeltUpdated", new BeltUpdate(
            @event.ProductId,
            @event.OrderNumber,
            @event.Priority.ToString(),
            true
        ));
    }
}
