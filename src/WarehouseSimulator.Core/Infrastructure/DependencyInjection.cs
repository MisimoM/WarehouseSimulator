using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.Infrastructure.Belt;
using WarehouseSimulator.Core.Infrastructure.Events;
using WarehouseSimulator.Core.Infrastructure.Persistence;
using WarehouseSimulator.Core.Infrastructure.Simulation;

namespace WarehouseSimulator.Core.Infrastructure;

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
