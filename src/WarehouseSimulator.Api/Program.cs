using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Api.Domain.Shared.Events;
using WarehouseSimulator.Api.Infrastructure.Events;
using WarehouseSimulator.Api.Infrastructure.Persistence;
using WarehouseSimulator.Api.Infrastructure.Persistence.Seed;
using WarehouseSimulator.Api.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ApplicationDbContext>("warehouse-db");
builder.Services.AddSingleton<IEventBus, EventBus>();
builder.Services.AddSignalR();


builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapHub<WarehouseHub>("/warehouse-hub");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(db);
}

app.Run();
