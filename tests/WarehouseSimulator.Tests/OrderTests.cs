using WarehouseSimulator.Core.Domain.Orders;
using WarehouseSimulator.Core.Domain.Shared;

namespace WarehouseSimulator.Tests;

public class OrderTests
{
    [Fact]
    public void Create_ShouldReturnPendingOrder()
    {
        var simulatedTime = DateTime.UtcNow;
        var order = Order.Create(Priority.Standard, DeliveryRegion.North, simulatedTime);

        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Equal(Priority.Standard, order.Priority);
        Assert.Equal(DeliveryRegion.North, order.Region);
    }

    [Fact]
    public void Create_ExpressOrder_ShouldHaveOneDayDeadline()
    {
        var simulatedTime = DateTime.UtcNow;
        var order = Order.Create(Priority.Express, DeliveryRegion.North, simulatedTime);

        Assert.Equal(simulatedTime.AddDays(1), order.DeliveryDeadline);
    }

    [Fact]
    public void Create_StandardOrder_ShouldHaveThreeDayDeadline()
    {
        var simulatedTime = DateTime.UtcNow;
        var order = Order.Create(Priority.Standard, DeliveryRegion.North, simulatedTime);

        Assert.Equal(simulatedTime.AddDays(3), order.DeliveryDeadline);
    }

    [Fact]
    public void AssignToTruck_ShouldSetTruckIdAndStatus()
    {
        var order = Order.Create(Priority.Standard, DeliveryRegion.North, DateTime.UtcNow);
        order.UpdateStatus(OrderStatus.InStorage);

        order.AssignToTruck(1);

        Assert.Equal(1, order.TruckId);
        Assert.Equal(OrderStatus.OnTruck, order.Status);
    }

    [Fact]
    public void Deliver_ShouldSetStatusToDelivered_WhenOnTruck()
    {
        var simulatedTime = DateTime.UtcNow;
        var order = Order.Create(Priority.Standard, DeliveryRegion.North, simulatedTime);
        order.AssignToTruck(1);

        var deliveredAt = simulatedTime.AddDays(2);
        order.Deliver(deliveredAt);

        Assert.Equal(OrderStatus.Delivered, order.Status);
        Assert.Equal(deliveredAt, order.DeliveredAt);
    }

    [Fact]
    public void Deliver_ShouldThrow_WhenNotOnTruck()
    {
        var order = Order.Create(Priority.Standard, DeliveryRegion.North, DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() => order.Deliver(DateTime.UtcNow));
    }

    [Fact]
    public void Deliver_IsOnTime_WhenDeliveredBeforeDeadline()
    {
        var simulatedTime = DateTime.UtcNow;
        var order = Order.Create(Priority.Standard, DeliveryRegion.North, simulatedTime);
        order.AssignToTruck(1);

        order.Deliver(simulatedTime.AddDays(2));

        Assert.True(order.DeliveredAt <= order.DeliveryDeadline);
    }

    [Fact]
    public void Deliver_IsLate_WhenDeliveredAfterDeadline()
    {
        var simulatedTime = DateTime.UtcNow;
        var order = Order.Create(Priority.Standard, DeliveryRegion.North, simulatedTime);
        order.AssignToTruck(1);

        order.Deliver(simulatedTime.AddDays(4));

        Assert.True(order.DeliveredAt > order.DeliveryDeadline);
    }
}
