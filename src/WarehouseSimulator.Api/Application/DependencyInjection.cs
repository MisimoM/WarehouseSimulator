using WarehouseSimulator.Api.Application.Services.Machines;
using WarehouseSimulator.Api.Application.Workers;

namespace WarehouseSimulator.Api.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMachineService, MachineService>();
        return services;
    }

    public static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddHostedService<OrderWorker>();
        services.AddHostedService<ProductionWorker>();
        services.AddHostedService<StorageWorker>();
        return services;
    }
}
