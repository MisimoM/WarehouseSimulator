namespace WarehouseSimulator.Api.Domain.Shared.Events;

public interface IEventHandler<T> where T : IDomainEvent
{
    Task Handle(T @event);
}