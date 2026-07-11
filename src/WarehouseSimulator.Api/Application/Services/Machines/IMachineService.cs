using WarehouseSimulator.Api.Domain.Machines;

namespace WarehouseSimulator.Api.Application.Services.Machines;

public interface IMachineService
{
    Task<List<Machine>> GetMachinesAsync();
    Task RepairMachineAsync(int machineId);
}
