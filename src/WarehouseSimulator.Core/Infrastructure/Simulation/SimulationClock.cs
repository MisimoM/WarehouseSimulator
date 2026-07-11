using WarehouseSimulator.Core.Domain.Shared;

namespace WarehouseSimulator.Core.Infrastructure.Simulation;

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

    public int GetRealMillisecondsFromHours(int simulatedHours)
    {
        return simulatedHours * _secondsPerHour * 1000;
    }

    public int GetRealMillisecondsFromMinutes(int simulatedMinutes)
    {
        return (int)(simulatedMinutes * (_secondsPerHour / 60.0) * 1000);
    }
}
