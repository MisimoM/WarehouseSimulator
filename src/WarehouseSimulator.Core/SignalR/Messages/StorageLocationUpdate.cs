using WarehouseSimulator.Core.Domain.Shared;

namespace WarehouseSimulator.Core.SignalR.Messages;

public record StorageLocationUpdate(
    int LocationId,
    string Row,
    int Column,
    string Status,
    Guid? ProductId,
    string? OrderNumber,
    string? Priority
);