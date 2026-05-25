namespace WarehouseSimulator.Api.Domain.Notifications;

public class Notification
{
    public int Id { get; private set; }
    public string Message { get; private set; } = null!;
    public NotificationType Type { get; private set; }
    public string Source { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime SimulatedCreatedAt { get; private set; }
    public bool IsRead { get; private set; }
    public bool IsResolved { get; private set; }

    private Notification() { }

    public static Notification Create(
        string message,
        NotificationType type,
        string source,
        DateTime simulatedTime)
    {
        return new Notification
        {
            Message = message,
            Type = type,
            Source = source,
            CreatedAt = DateTime.UtcNow,
            SimulatedCreatedAt = simulatedTime
        };
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }

    public void Resolve()
    {
        if (Type is not NotificationType.Error)
            throw new InvalidOperationException("Only errors can be resolved.");

        IsResolved = true;
    }
}
