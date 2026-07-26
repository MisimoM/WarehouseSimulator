namespace WarehouseSimulator.Core.Domain.Shared;

public interface ISimulationClock
{
    DateTime GetCurrentSimulatedTime();
    Task Delay(TimeSpan simulatedDuration);
    Task Delay(TimeSpan simulatedDuration, CancellationToken cancellationToken);
}