namespace WarehouseSimulator.Api.Domain.Shared.Events;

public interface IEventBus
{
    Task PublishAsync<T>(T @event) where T : IDomainEvent;
}
