using WarehouseSimulator.Core.Domain.Shared.Events;

namespace WarehouseSimulator.Core.Domain.StorageLocations.Events;

public class StorageLocationUpdatedEvent : IDomainEvent
{
    public int LocationId { get; }
    public string Row { get; }
    public int Column { get; }
    public StorageLocationStatus Status { get; }
    public Guid? ProductId { get; }
    public string? OrderNumber { get; }
    public DateTime OccurredAt { get; }
    public DateTime SimulatedOccurredAt { get; }

    public StorageLocationUpdatedEvent(
        int locationId,
        string row,
        int column,
        StorageLocationStatus status,
        Guid? productId,
        string? orderNumber,
        DateTime simulatedTime)
    {
        LocationId = locationId;
        Row = row;
        Column = column;
        Status = status;
        ProductId = productId;
        OrderNumber = orderNumber;
        OccurredAt = DateTime.UtcNow;
        SimulatedOccurredAt = simulatedTime;
    }
}