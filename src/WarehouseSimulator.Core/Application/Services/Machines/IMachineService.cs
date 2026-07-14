using WarehouseSimulator.Core.Domain.Machines;

namespace WarehouseSimulator.Core.Application.Services.Machines;

public interface IMachineService
{
    Task<List<MachineView>> GetMachinesAsync();
    Task RepairMachineAsync(int machineId);
}
