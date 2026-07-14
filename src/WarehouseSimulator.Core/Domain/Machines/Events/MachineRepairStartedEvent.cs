using WarehouseSimulator.Core.Domain.Shared.Events;

namespace WarehouseSimulator.Core.Domain.Machines.Events;

public class MachineRepairStartedEvent : IDomainEvent
{
    public int MachineId { get; }
    public MachineType MachineType { get; }

    public MachineRepairStartedEvent(int machineId, MachineType machineType)
    {
        MachineId = machineId;
        MachineType = machineType;
    }
}
