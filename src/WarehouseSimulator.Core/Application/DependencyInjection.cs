using Microsoft.Extensions.DependencyInjection;
using WarehouseSimulator.Core.Application.EventHandlers.Machines;
using WarehouseSimulator.Core.Application.Services.Machines;
using WarehouseSimulator.Core.Application.Workers;
using WarehouseSimulator.Core.Domain.Machines.Events;
using WarehouseSimulator.Core.Domain.Shared.Events;

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

        // Services
        services.AddScoped<IMachineService, MachineService>();
        return services;
    }
}
