using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.Domain.Trucks;

namespace WarehouseSimulator.Core.Domain.Orders;

public class Order
{
    public Guid Id { get; private set; }
    public int OrderNumber { get; private set; }
    public Priority Priority { get; private set; }
    public OrderStatus Status { get; private set; }
    public DeliveryRegion Region { get; private set; }
    public DateTime DeliveryDeadline { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int? TruckId { get; private set; }

    public Truck? Truck { get; private set; }

    public string DisplayNumber => $"#{OrderNumber:D4}";

    private Order() { }

    public static Order Create(Priority priority, DeliveryRegion region, DateTime simulatedTime)
    {
        var deliveryDeadline = priority == Priority.Express
            ? simulatedTime.AddDays(1)
            : simulatedTime.AddDays(3);

        return new Order
        {
            Id = Guid.NewGuid(),
            Priority = priority,
            Region = region,
            Status = OrderStatus.Pending,
            DeliveryDeadline = deliveryDeadline,
            CreatedAt = simulatedTime
        };
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }

    public void AssignToTruck(int truckId)
    {
        TruckId = truckId;
        Status = OrderStatus.OnTruck;
    }

    public void Deliver(DateTime simulatedTime)
    {
        if (Status is not OrderStatus.OnTruck)
            throw new InvalidOperationException("Order must be on truck to be delivered.");

        Status = OrderStatus.Delivered;
        DeliveredAt = simulatedTime;
    }
}