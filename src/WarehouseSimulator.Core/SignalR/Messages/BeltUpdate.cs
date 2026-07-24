namespace WarehouseSimulator.Core.SignalR.Messages;

public record BeltUpdate(
    Guid ProductId,
    int? OrderNumber,
    string? Priority,
    bool IsAdded
);
