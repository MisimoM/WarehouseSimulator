namespace WarehouseSimulator.Core.Application.Services.StorageLocations;

public class StorageLocationView
{
    public int Id { get; set; }
    public string Row { get; set; } = null!;
    public int Column { get; set; }
    public string Status { get; set; } = null!;
    public int? OrderNumber { get; set; }

    public string LocationCode => $"{Row}{Column}";
    public string? DisplayNumber => OrderNumber.HasValue ? $"#{OrderNumber:D4}" : null;
}
