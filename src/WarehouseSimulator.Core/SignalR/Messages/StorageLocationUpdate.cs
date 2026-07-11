namespace WarehouseSimulator.Core.SignalR.Messages;

public record StorageLocationUpdate(
int LocationId,
string LocationCode,
string Status,
Guid? ProductId,
int? OrderNumber
);
