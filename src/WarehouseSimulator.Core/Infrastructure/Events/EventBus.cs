using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseSimulator.Core.Domain.Shared.Events;

namespace WarehouseSimulator.Core.Infrastructure.Events;

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
        using var scope = _serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>();
        
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