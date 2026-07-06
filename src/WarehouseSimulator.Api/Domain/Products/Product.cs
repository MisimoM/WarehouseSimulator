using WarehouseSimulator.Api.Domain.Orders;
using WarehouseSimulator.Api.Domain.Shared;

namespace WarehouseSimulator.Api.Domain.Products;

public class Product
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Priority Priority { get; private set; }
    public ProductStatus Status { get; private set; }
    public DateTime ProducedAt { get; private set; }
    public DateTime SimulatedProducedAt { get; private set; }
    public DateTime? StoredAt { get; private set; }
    public DateTime? SimulatedStoredAt { get; private set; }

    public Order Order { get; private set; } = null!;

    private Product() { }

    public static Product Create(Order order, DateTime simulatedTime)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Priority = order.Priority,
            Status = ProductStatus.OnBelt,
            ProducedAt = DateTime.UtcNow,
            SimulatedProducedAt = simulatedTime
        };
    }

    public void MarkAsBeingStored()
    {
        if (Status is not ProductStatus.OnBelt)
            throw new InvalidOperationException($"Product must be OnBelt to be stored, current status: {Status}");

        Status = ProductStatus.BeingStored;
    }

    public void MarkAsInStorage(DateTime simulatedTime)
    {
        if (Status is not ProductStatus.BeingStored)
            throw new InvalidOperationException($"Product must be BeingStored to be stored, current status: {Status}");

        Status = ProductStatus.InStorage;
        StoredAt = DateTime.UtcNow;
        SimulatedStoredAt = simulatedTime;
    }
}
