using Microsoft.Extensions.DependencyInjection;
using WarehouseSimulator.Core.Application.EventHandlers.Machines;
using WarehouseSimulator.Core.Application.EventHandlers.StorageLocations;
using WarehouseSimulator.Core.Application.Services.Machines;
using WarehouseSimulator.Core.Application.Services.StorageLocations;
using WarehouseSimulator.Core.Application.Workers;
using WarehouseSimulator.Core.Domain.Machines.Events;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.Domain.StorageLocations.Events;

namespace WarehouseSimulator.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Workers
        services.AddHostedService<OrderWorker>();
        services.AddHostedService<ProductionWorker>();
        services.AddHostedService<StorageWorker>();

        // Event handlers
        services.AddScoped<IEventHandler<MachineBreakdownEvent>, MachineBreakdownHandler>();
        services.AddScoped<IEventHandler<MachineRepairStartedEvent>, MachineRepairStartedEventHandler>();
        services.AddScoped<IEventHandler<MachineRepairedEvent>, MachineRepairedEventHandler>();
        services.AddScoped<IEventHandler<StorageLocationUpdatedEvent>, StorageLocationUpdatedEventHandler>();

        // Services
        services.AddScoped<IMachineService, MachineService>();
        services.AddScoped<IStorageLocationService, StorageLocationService>();
        return services;
    }
}
