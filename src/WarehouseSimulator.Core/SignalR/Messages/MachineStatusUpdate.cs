namespace WarehouseSimulator.Core.SignalR.Messages;

public record MachineStatusUpdate(
    int MachineId,
    string MachineType,
    string Status,
    int? TotalBreakdowns
);
