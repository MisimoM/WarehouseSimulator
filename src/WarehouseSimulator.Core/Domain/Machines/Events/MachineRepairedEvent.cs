using WarehouseSimulator.Core.Domain.Shared.Events;

namespace WarehouseSimulator.Core.Domain.Machines.Events;

public class MachineRepairedEvent : IDomainEvent
{
    public int MachineId { get; }
    public MachineType MachineType { get; }

    public MachineRepairedEvent(int machineId, MachineType machineType)
    {
        MachineId = machineId;
        MachineType = machineType;
    }
}
