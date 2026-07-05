using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Api.Application.Workers;
using WarehouseSimulator.Api.Domain.Shared;
using WarehouseSimulator.Api.Domain.Shared.Events;
using WarehouseSimulator.Api.Infrastructure.Belt;
using WarehouseSimulator.Api.Infrastructure.Events;
using WarehouseSimulator.Api.Infrastructure.Persistence;
using WarehouseSimulator.Api.Infrastructure.Persistence.Seed;
using WarehouseSimulator.Api.Infrastructure.Simulation;
using WarehouseSimulator.Api.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Aspire
builder.AddServiceDefaults();

// Database
builder.AddNpgsqlDbContext<ApplicationDbContext>("warehouse-db");
builder.Services.AddDbContextFactory<ApplicationDbContext>();

// Services
builder.Services.AddSingleton<IEventBus, EventBus>();
builder.Services.AddSingleton<ISimulationClock, SimulationClock>();
builder.Services.AddSingleton<BeltChannel>();
builder.Services.AddScoped<BeltRestoreService>();

// SignalR
builder.Services.AddSignalR();

// Workers
builder.Services.AddHostedService<OrderWorker>();
builder.Services.AddHostedService<ProductionWorker>();

// OpenApi
builder.Services.AddOpenApi();

var app = builder.Build();

// Middleware
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Hubs
app.MapHub<WarehouseHub>("/warehouse-hub");

// Database migration and seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(db);

    var beltRestore = scope.ServiceProvider.GetRequiredService<BeltRestoreService>();
    await beltRestore.RestoreAsync();
}

app.Run();