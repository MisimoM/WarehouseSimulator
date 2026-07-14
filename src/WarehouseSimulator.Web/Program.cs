using Microsoft.EntityFrameworkCore;
using WarehouseSimulator.Core.Application;
using WarehouseSimulator.Core.Infrastructure;
using WarehouseSimulator.Core.Infrastructure.Belt;
using WarehouseSimulator.Core.Infrastructure.Persistence;
using WarehouseSimulator.Core.Infrastructure.Persistence.Seed;
using WarehouseSimulator.Core.SignalR;
using WarehouseSimulator.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddSignalR();
builder.Services.AddScoped<WarehouseHubClient>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
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

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
