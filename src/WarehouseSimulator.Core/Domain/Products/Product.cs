using WarehouseSimulator.Core.Domain.Orders;
using WarehouseSimulator.Core.Domain.Shared;

namespace WarehouseSimulator.Core.Domain.Products;

public class Product
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Priority Priority { get; private set; }
    public ProductStatus Status { get; private set; }
    public DateTime ProducedAt { get; private set; }
    public DateTime? StoredAt { get; private set; }
    public DateTime? PickedAt { get; private set; }

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
            ProducedAt = simulatedTime
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
        StoredAt = simulatedTime;
    }

    public void MarkAsBeingPicked()
    {
        if (Status is not ProductStatus.InStorage)
            throw new InvalidOperationException($"Product must be InStorage to be picked, current status: {Status}");

        Status = ProductStatus.BeingPicked;
    }

    public void MarkAsOnTruck(DateTime simulatedTime)
    {
        if (Status is not ProductStatus.BeingPicked)
            throw new InvalidOperationException($"Product must be BeingPicked to be on truck, current status: {Status}");

        Status = ProductStatus.OnTruck;
        PickedAt = simulatedTime;
    }
}