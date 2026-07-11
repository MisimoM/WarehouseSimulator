using WarehouseSimulator.Core.Domain.Shared;

namespace WarehouseSimulator.Core.Domain.Orders;

public class Order
{
    public Guid Id { get; private set; }
    public int OrderNumber { get; private set; }
    public Priority Priority { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime SimulatedCreatedAt { get; private set; }

    public string DisplayNumber => $"#{OrderNumber:D4}";

    private Order() { }

    public static Order Create(Priority priority, DateTime simulatedTime)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            Priority = priority,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            SimulatedCreatedAt = simulatedTime
        };
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }
}

