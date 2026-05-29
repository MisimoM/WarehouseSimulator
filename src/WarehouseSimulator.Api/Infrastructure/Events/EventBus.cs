using WarehouseSimulator.Api.Domain.Shared.Events;

namespace WarehouseSimulator.Api.Infrastructure.Events;

public class EventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventBus> _logger;

    public EventBus(IServiceProvider serviceProvider, ILogger<EventBus> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T @event) where T : IDomainEvent
    {
        var handlers = _serviceProvider.GetServices<IEventHandler<T>>();
        
        if (!handlers.Any())
            return;

        foreach (var handler in handlers)
        {
            try
            {
                await handler.Handle(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Handler failed for event {EventType}", typeof(T).Name);
            }
        }
    }
}