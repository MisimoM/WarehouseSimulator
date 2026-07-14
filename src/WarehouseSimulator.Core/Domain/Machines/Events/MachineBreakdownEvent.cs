using WarehouseSimulator.Core.Domain.Shared.Events;

namespace WarehouseSimulator.Core.Domain.Machines.Events;

public class MachineBreakdownEvent : IDomainEvent
{
    public int MachineId { get; }
    public MachineType MachineType { get; }
    public int TotalBreakdowns { get; }

    public MachineBreakdownEvent(int machineId, MachineType machineType, int totalBreakdowns)
    {
        MachineId = machineId;
        MachineType = machineType;
        TotalBreakdowns = totalBreakdowns;
    }
}
