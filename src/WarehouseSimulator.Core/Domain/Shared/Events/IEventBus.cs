namespace WarehouseSimulator.Core.Domain.Shared.Events;

public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : IDomainEvent;
}
