namespace WarehouseSimulator.Api.Domain.Shared;

public interface ISimulationClock
{
    DateTime GetCurrentSimulatedTime();
}
