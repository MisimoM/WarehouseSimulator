using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Api.Application;
using WarehouseSimulator.Api.Infrastructure;
using WarehouseSimulator.Api.Infrastructure.Belt;
using WarehouseSimulator.Api.Infrastructure.Persistence;
using WarehouseSimulator.Api.Infrastructure.Persistence.Seed;
using WarehouseSimulator.Api.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Aspire
builder.AddServiceDefaults();

// Infrastructure services
builder.AddInfrastructure();

// SignalR
builder.Services.AddSignalR();

// Application services and workers
builder.Services.AddApplication();
builder.Services.AddWorkers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7286")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

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

// Cors
app.UseCors();

app.Run();