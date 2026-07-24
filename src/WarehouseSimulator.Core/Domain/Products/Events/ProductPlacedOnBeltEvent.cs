using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.Domain.Shared.Events;

namespace WarehouseSimulator.Core.Domain.Products.Events;

public class ProductPlacedOnBeltEvent : IDomainEvent
{
    public Guid ProductId { get; }
    public int OrderNumber { get; }
    public Priority Priority { get; }

    public ProductPlacedOnBeltEvent(Guid productId, int orderNumber, Priority priority)
    {
        ProductId = productId;
        OrderNumber = orderNumber;
        Priority = priority;
    }
}
