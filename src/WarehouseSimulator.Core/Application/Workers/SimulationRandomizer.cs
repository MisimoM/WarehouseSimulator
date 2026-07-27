using WarehouseSimulator.Core.Domain.Shared;

namespace WarehouseSimulator.Core.Application.Workers;

public class SimulationRandomizer
{
    public static bool ShouldBreakDown(int chancePercent = 10)
    {
        return Random.Shared.Next(0, 100) < chancePercent;
    }

    public static bool ShouldTruckBeDelayed(int chancePercent = 15)
    {
        return Random.Shared.Next(0, 100) < chancePercent;
    }

    public static Priority GetOrderPriority(int chancePercent = 20)
    {
        return Random.Shared.Next(0, 100) < chancePercent
            ? Priority.Express
            : Priority.Standard;
    }

    public static DeliveryRegion GetRandomRegion()
    {
        return (DeliveryRegion)Random.Shared.Next(0, 3);
    }
}
