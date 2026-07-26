using WarehouseSimulator.Core.Domain.Shared;

namespace WarehouseSimulator.Core.Infrastructure.Simulation;

public class SimulationClock(int realSecondsPerSimulatedHour = 10) : ISimulationClock
{
    private readonly DateTime _realStartTime = DateTime.UtcNow;
    private readonly DateTime _simulatedStartTime = DateTime.UtcNow;
    private readonly int _realSecondsPerSimulatedHour = realSecondsPerSimulatedHour;

    public DateTime GetCurrentSimulatedTime()
    {
        var realElapsed = DateTime.UtcNow - _realStartTime;

        var simulatedHours = realElapsed.TotalSeconds / _realSecondsPerSimulatedHour;

        return _simulatedStartTime.AddHours(simulatedHours);
    }

    private int GetRealMilliseconds(TimeSpan simulatedDuration)
    {
        var realMillisecondsPerSimulatedHour = _realSecondsPerSimulatedHour * 1000;

        var millisecondsPerSimulatedHour = TimeSpan.FromHours(1).TotalMilliseconds;

        var multiplier = realMillisecondsPerSimulatedHour / millisecondsPerSimulatedHour;

        return (int)(simulatedDuration.TotalMilliseconds * multiplier);
    }

    public Task Delay(TimeSpan simulatedDuration, CancellationToken cancellationToken)
    {
        var realMilliseconds = GetRealMilliseconds(simulatedDuration);

        return Task.Delay(realMilliseconds, cancellationToken);
    }

    public Task Delay(TimeSpan simulatedDuration)
    {
        var realMilliseconds = GetRealMilliseconds(simulatedDuration);

        return Task.Delay(realMilliseconds);
    }
}