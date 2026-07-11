using WarehouseSimulator.Api.Domain.Shared;
using WarehouseSimulator.Api.Domain.Shared.Events;
using WarehouseSimulator.Api.Infrastructure.Belt;
using WarehouseSimulator.Api.Infrastructure.Events;
using WarehouseSimulator.Api.Infrastructure.Persistence;
using WarehouseSimulator.Api.Infrastructure.Simulation;

namespace WarehouseSimulator.Api.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ApplicationDbContext>("warehouse-db");
        builder.Services.AddDbContextFactory<ApplicationDbContext>();
        
        builder.Services.AddSingleton<IEventBus, EventBus>();
        builder.Services.AddSingleton<ISimulationClock, SimulationClock>();
        builder.Services.AddSingleton<BeltChannel>();
        
        builder.Services.AddScoped<BeltRestoreService>();
        
        return builder;
    }
}
