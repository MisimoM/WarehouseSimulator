using WarehouseSimulator.Core.Domain.Machines;

namespace WarehouseSimulator.Tests;

public class MachineTests
{
    [Fact]
    public void Create_ShouldReturnRunningMachine()
    {
        var machine = Machine.Create(MachineType.Production);

        Assert.Equal(MachineStatus.Running, machine.Status);
        Assert.Equal(0, machine.TotalBreakdowns);
    }

    [Fact]
    public void Break_ShouldSetStatusToBroken_WhenRunning()
    {
        var machine = Machine.Create(MachineType.Production);

        machine.Break(DateTime.UtcNow);

        Assert.Equal(MachineStatus.Broken, machine.Status);
        Assert.Equal(1, machine.TotalBreakdowns);
    }

    [Fact]
    public void Break_ShouldThrow_WhenNotRunning()
    {
        var machine = Machine.Create(MachineType.Production);
        machine.Break(DateTime.UtcNow);

        Assert.Throws<InvalidOperationException>(() => machine.Break(DateTime.UtcNow));
    }

    [Fact]
    public void StartRepair_ShouldSetStatusToRepairing_WhenBroken()
    {
        var machine = Machine.Create(MachineType.Production);
        machine.Break(DateTime.UtcNow);

        machine.StartRepair();

        Assert.Equal(MachineStatus.Repairing, machine.Status);
    }

    [Fact]
    public void StartRepair_ShouldThrow_WhenNotBroken()
    {
        var machine = Machine.Create(MachineType.Production);

        Assert.Throws<InvalidOperationException>(() => machine.StartRepair());
    }

    [Fact]
    public void FinishRepair_ShouldSetStatusToRunning_WhenRepairing()
    {
        var machine = Machine.Create(MachineType.Production);
        machine.Break(DateTime.UtcNow);
        machine.StartRepair();

        machine.FinishRepair(DateTime.UtcNow);

        Assert.Equal(MachineStatus.Running, machine.Status);
    }

    [Fact]
    public void FinishRepair_ShouldThrow_WhenNotRepairing()
    {
        var machine = Machine.Create(MachineType.Production);

        Assert.Throws<InvalidOperationException>(() => machine.FinishRepair(DateTime.UtcNow));
    }

    [Fact]
    public void Break_ShouldIncrementTotalBreakdowns_EachTime()
    {
        var machine = Machine.Create(MachineType.Production);

        machine.Break(DateTime.UtcNow);
        machine.StartRepair();
        machine.FinishRepair(DateTime.UtcNow);
        machine.Break(DateTime.UtcNow);

        Assert.Equal(2, machine.TotalBreakdowns);
    }
}
