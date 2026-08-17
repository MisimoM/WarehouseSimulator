using WarehouseSimulator.Core.Domain.StorageLocations;

namespace WarehouseSimulator.Tests;

public class StorageLocationTests
{
    [Fact]
    public void Create_ShouldReturnEmptyStorageLocation()
    {
        var location = StorageLocation.Create("A", 1);

        Assert.Equal(StorageLocationStatus.Empty, location.Status);
        Assert.Equal("A", location.Row);
        Assert.Equal(1, location.Column);
        Assert.Null(location.ProductId);
    }

    [Fact]
    public void Create_ShouldReturnCorrectLocationCode()
    {
        var location = StorageLocation.Create("A", 1);

        Assert.Equal("A1", location.LocationCode);
    }

    [Fact]
    public void Reserve_ShouldSetStatusToReserved()
    {
        var location = StorageLocation.Create("A", 1);

        location.Reserve();

        Assert.Equal(StorageLocationStatus.Reserved, location.Status);
    }

    [Fact]
    public void Occupy_ShouldSetStatusToOccupiedAndProductId()
    {
        var location = StorageLocation.Create("A", 1);
        var productId = Guid.NewGuid();

        location.Occupy(productId);

        Assert.Equal(StorageLocationStatus.Occupied, location.Status);
        Assert.Equal(productId, location.ProductId);
    }

    [Fact]
    public void Clear_ShouldSetStatusToEmptyAndRemoveProductId()
    {
        var location = StorageLocation.Create("A", 1);
        var productId = Guid.NewGuid();
        location.Occupy(productId);

        location.Clear();

        Assert.Equal(StorageLocationStatus.Empty, location.Status);
        Assert.Null(location.ProductId);
    }
}