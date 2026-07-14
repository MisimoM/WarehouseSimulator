using WarehouseSimulator.Core.Domain.Machines;

namespace WarehouseSimulator.Core.Application.Services.Machines;

public class MachineView
{
    public int Id { get; set; }
    public MachineType Type { get; set; }
    public MachineStatus Status { get; set; }
    public int TotalBreakdowns { get; set; }
}
