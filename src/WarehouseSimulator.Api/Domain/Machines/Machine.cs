namespace WarehouseSimulator.Api.Domain.Machines;

public class Machine
{
    public int Id { get; private set; }
    public MachineType Type { get; private set; }
    public MachineStatus Status { get; private set; }
    public DateTime? LastBrokenAt { get; private set; }
    public DateTime? SimulatedLastBrokenAt { get; private set; }
    public DateTime? LastRepairedAt { get; private set; }
    public DateTime? SimulatedLastRepairedAt { get; private set; }
    public int TotalBreakdowns { get; private set; }

    private Machine() { }

    public static Machine Create(MachineType type)
    {
        return new Machine
        {
            Type = type,
            Status = MachineStatus.Running,
            TotalBreakdowns = 0
        };
    }

    public void Break(DateTime simulatedTime)
    {
        if (Status is not MachineStatus.Running)
            throw new InvalidOperationException("Machine must be running to break down.");

        Status = MachineStatus.Broken;
        LastBrokenAt = DateTime.UtcNow;
        SimulatedLastBrokenAt = simulatedTime;
        TotalBreakdowns++;
    }

    public void StartRepair()
    {
        if (Status is not MachineStatus.Broken)
            throw new InvalidOperationException("Machine must be broken to start repair.");

        Status = MachineStatus.Repairing;
    }

    public void FinishRepair(DateTime simulatedTime)
    {
        if (Status is not MachineStatus.Repairing)
            throw new InvalidOperationException("Machine must be repairing to finish repair.");

        Status = MachineStatus.Running;
        LastRepairedAt = DateTime.UtcNow;
        SimulatedLastRepairedAt = simulatedTime;
    }
}
