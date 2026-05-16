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

    public static Product Create(Guid orderId, Priority priority, DateTime simulatedTime)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Priority = priority,
            Status = ProductStatus.OnBelt,
            ProducedAt = DateTime.UtcNow,
            SimulatedProducedAt = simulatedTime
        };
    }
}
