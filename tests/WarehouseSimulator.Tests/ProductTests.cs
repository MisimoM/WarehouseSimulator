using WarehouseSimulator.Core.Domain.Orders;
using WarehouseSimulator.Core.Domain.Products;
using WarehouseSimulator.Core.Domain.Shared;

namespace WarehouseSimulator.Tests;

public class ProductTests
{
    private static Order CreateOrder() =>
        Order.Create(Priority.Standard, DeliveryRegion.North, DateTime.UtcNow);

    [Fact]
    public void Create_ShouldReturnProductOnBelt()
    {
        var order = CreateOrder();
        var simulatedTime = DateTime.UtcNow;

        var product = Product.Create(order, simulatedTime);

        Assert.Equal(ProductStatus.OnBelt, product.Status);
        Assert.Equal(order.Priority, product.Priority);
        Assert.Equal(order.Id, product.OrderId);
        Assert.Equal(simulatedTime, product.ProducedAt);
    }

    [Fact]
    public void MarkAsBeingStored_ShouldSetStatusToBeingStored_WhenOnBelt()
    {
        var order = CreateOrder();
        var product = Product.Create(order, DateTime.UtcNow);

        product.MarkAsBeingStored();

        Assert.Equal(ProductStatus.BeingStored, product.Status);
    }

    [Fact]
    public void MarkAsBeingStored_ShouldThrow_WhenNotOnBelt()
    {
        var order = CreateOrder();
        var product = Product.Create(order, DateTime.UtcNow);
        product.MarkAsBeingStored();

        Assert.Throws<InvalidOperationException>(() => product.MarkAsBeingStored());
    }

    [Fact]
    public void MarkAsInStorage_ShouldSetStatusAndStoredAt_WhenBeingStored()
    {
        var order = CreateOrder();
        var product = Product.Create(order, DateTime.UtcNow);
        product.MarkAsBeingStored();
        var simulatedTime = DateTime.UtcNow;

        product.MarkAsInStorage(simulatedTime);

        Assert.Equal(ProductStatus.InStorage, product.Status);
        Assert.Equal(simulatedTime, product.StoredAt);
    }

    [Fact]
    public void MarkAsInStorage_ShouldThrow_WhenNotBeingStored()
    {
        var order = CreateOrder();
        var product = Product.Create(order, DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() => product.MarkAsInStorage(DateTime.UtcNow));
    }

    [Fact]
    public void MarkAsBeingPicked_ShouldSetStatusToBeingPicked_WhenInStorage()
    {
        var order = CreateOrder();
        var product = Product.Create(order, DateTime.UtcNow);
        product.MarkAsBeingStored();
        product.MarkAsInStorage(DateTime.UtcNow);

        product.MarkAsBeingPicked();

        Assert.Equal(ProductStatus.BeingPicked, product.Status);
    }

    [Fact]
    public void MarkAsBeingPicked_ShouldThrow_WhenNotInStorage()
    {
        var order = CreateOrder();
        var product = Product.Create(order, DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() => product.MarkAsBeingPicked());
    }

    [Fact]
    public void MarkAsOnTruck_ShouldSetStatusAndPickedAt_WhenBeingPicked()
    {
        var order = CreateOrder();
        var product = Product.Create(order, DateTime.UtcNow);
        product.MarkAsBeingStored();
        product.MarkAsInStorage(DateTime.UtcNow);
        product.MarkAsBeingPicked();
        var simulatedTime = DateTime.UtcNow;

        product.MarkAsOnTruck(simulatedTime);

        Assert.Equal(ProductStatus.OnTruck, product.Status);
        Assert.Equal(simulatedTime, product.PickedAt);
    }

    [Fact]
    public void MarkAsOnTruck_ShouldThrow_WhenNotBeingPicked()
    {
        var order = CreateOrder();
        var product = Product.Create(order, DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() => product.MarkAsOnTruck(DateTime.UtcNow));
    }
}
