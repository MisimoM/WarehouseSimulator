using WarehouseSimulator.Core.Domain.Products;

namespace WarehouseSimulator.Core.Domain.StorageLocations;

public class StorageLocation
{
    public int Id { get; private set; }
    public Guid? ProductId { get; private set; }
    public string Row { get; private set; } = null!;
    public int Column { get; private set; }
    public string LocationCode => $"{Row}{Column}";
    public uint LocationVersion { get; private set; }
    public StorageLocationStatus Status { get; private set; }

    public Product? Product { get; private set; }

    private StorageLocation() { }

    public static StorageLocation Create(string row, int column)
    {
        return new StorageLocation
        {
            Row = row,
            Column = column,
            Status = StorageLocationStatus.Empty
        };
    }

    public void Reserve()
    {
        Status = StorageLocationStatus.Reserved;
    }

    public void Occupy(Guid productId)
    {
        ProductId = productId;
        Status = StorageLocationStatus.Occupied;
    }

    public void Clear()
    {
        ProductId = null;
        Status = StorageLocationStatus.Empty;
    }

}
