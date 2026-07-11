using Microsoft.Extensions.DependencyInjection;
using WarehouseSimulator.Api.Application.Workers;
using WarehouseSimulator.Core.Application.Services.Machines;
using WarehouseSimulator.Core.Application.Workers;

namespace WarehouseSimulator.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddHostedService<OrderWorker>();
        services.AddHostedService<ProductionWorker>();
        services.AddHostedService<StorageWorker>();

        services.AddScoped<IMachineService, MachineService>();
        return services;
    }
}
