namespace WarehouseSimulator.Core.Domain.Shared;

public interface ISimulationClock
{
    DateTime GetCurrentSimulatedTime();
    int GetRealMillisecondsFromHours(int simulatedHours);
    int GetRealMillisecondsFromMinutes(int simulatedMinutes);
}
