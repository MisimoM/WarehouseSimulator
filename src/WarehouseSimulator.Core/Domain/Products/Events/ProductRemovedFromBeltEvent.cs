using WarehouseSimulator.Core.Domain.Shared.Events;

namespace WarehouseSimulator.Core.Domain.Products.Events;

public class ProductRemovedFromBeltEvent : IDomainEvent
{
    public Guid ProductId { get; }

    public ProductRemovedFromBeltEvent(Guid productId)
    {
        ProductId = productId;
    }
}
