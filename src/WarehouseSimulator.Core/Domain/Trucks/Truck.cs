using WarehouseSimulator.Core.Domain.Shared;

namespace WarehouseSimulator.Core.Domain.Trucks;

public class Truck
{
    public const int MaxCapacity = 10;

    public int Id { get; private set; }
    public TruckStatus Status { get; private set; }
    public DeliveryRegion Region { get; private set; }
    public DateTime? DeparturedAt { get; private set; }

    private Truck() { }

    public static Truck Create(DeliveryRegion region)
    {
        return new Truck
        {
            Region = region,
            Status = TruckStatus.Loading
        };
    }

    public void Depart(DateTime simulatedTime)
    {
        if (Status is not TruckStatus.Loading)
            throw new InvalidOperationException("Truck must be loading to depart.");

        Status = TruckStatus.Departed;
        DeparturedAt = simulatedTime;
    }

    public void Delay(DateTime simulatedTime)
    {
        Status = TruckStatus.Delayed;
        DeparturedAt = simulatedTime;
    }
}
