namespace WarehouseSimulator.Api.Application.Workers;

public class SimulationRandomizer
{
    public static bool ShouldBreakDown(int chancePercent = 10)
    {
        return Random.Shared.Next(0, 100) < chancePercent;
    }
}
