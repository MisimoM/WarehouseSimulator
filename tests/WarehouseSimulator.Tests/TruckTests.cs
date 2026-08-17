using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.Domain.Trucks;

namespace WarehouseSimulator.Tests;

public class TruckTests
{
    [Fact]
    public void Create_ShouldReturnLoadingTruck()
    {
        var truck = Truck.Create(DeliveryRegion.North);

        Assert.Equal(TruckStatus.Loading, truck.Status);
        Assert.Equal(DeliveryRegion.North, truck.Region);
        Assert.Null(truck.DeparturedAt);
    }

    [Fact]
    public void Depart_ShouldSetStatusToDeparted_WhenLoading()
    {
        var truck = Truck.Create(DeliveryRegion.North);
        var simulatedTime = DateTime.UtcNow;

        truck.Depart(simulatedTime);

        Assert.Equal(TruckStatus.Departed, truck.Status);
        Assert.Equal(simulatedTime, truck.DeparturedAt);
    }

    [Fact]
    public void Depart_ShouldThrow_WhenNotLoading()
    {
        var truck = Truck.Create(DeliveryRegion.North);
        truck.Depart(DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() => truck.Depart(DateTime.UtcNow));
    }

    [Fact]
    public void Delay_ShouldSetStatusToDelayed()
    {
        var truck = Truck.Create(DeliveryRegion.North);
        var simulatedTime = DateTime.UtcNow;

        truck.Delay(simulatedTime);

        Assert.Equal(TruckStatus.Delayed, truck.Status);
        Assert.Equal(simulatedTime, truck.DeparturedAt);
    }

    [Fact]
    public void MaxCapacity_ShouldBeTen()
    {
        Assert.Equal(10, Truck.MaxCapacity);
    }
}
