using WarehouseSimulator.Api.Domain.Shared;

namespace WarehouseSimulator.Api.Infrastructure.Simulation;

public class SimulationClock(int secondsPerHour = 6) : ISimulationClock
{
    private readonly DateTime _realStartTime = DateTime.UtcNow;
    private readonly DateTime _simulatedStartTime = DateTime.UtcNow;
    private readonly int _secondsPerHour = secondsPerHour;

    public DateTime GetCurrentSimulatedTime()
    {
        var realElapsed = DateTime.UtcNow - _realStartTime;
        var simulatedHours = realElapsed.TotalSeconds / _secondsPerHour;
        return _simulatedStartTime.AddHours(simulatedHours);
    }

    public int GetRealMilliseconds(int simulatedHours)
    {
        return simulatedHours * _secondsPerHour * 1000;
    }
}
