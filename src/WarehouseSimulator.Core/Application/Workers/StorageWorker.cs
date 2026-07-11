using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WarehouseSimulator.Core.Domain.Machines;
using WarehouseSimulator.Core.Domain.Orders;
using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.Domain.StorageLocations;
using WarehouseSimulator.Core.Infrastructure.Belt;
using WarehouseSimulator.Core.Infrastructure.Persistence;
using WarehouseSimulator.Core.SignalR;
using WarehouseSimulator.Core.SignalR.Messages;

namespace WarehouseSimulator.Api.Application.Workers;

public class StorageWorker(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    BeltChannel belt,
    ISimulationClock simulationClock,
    IHubContext<WarehouseHub> hub,
    ILogger<StorageWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var machine = await context.Machines
                .FirstAsync(m => m.Type == MachineType.Storage, cancellationToken);

            if (machine.Status != MachineStatus.Running)
            {
                logger.LogWarning("Storage machine is not running, waiting...");
                await Task.Delay(simulationClock.GetRealMillisecondsFromHours(1), cancellationToken);
                continue;
            }

            var product = await belt.Reader.ReadAsync(cancellationToken);

            var order = await context.Orders.FindAsync(product.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"Order not found for product {product.Id}");

            logger.LogInformation("Product {ProductId} picked from belt", product.Id);

            var location = await context.StorageLocations
                .FirstOrDefaultAsync(s => s.Status == StorageLocationStatus.Empty, cancellationToken);

            if (location is null)
            {
                logger.LogWarning("No empty storage locations available");
                await Task.Delay(1000, cancellationToken);
                continue;
            }

            while (true)
            {
                // Reserve the location and mark the product as being stored
                location.Reserve();
                product.MarkAsBeingStored();
                await context.SaveChangesAsync(cancellationToken);

                // Simulate the time it takes to store the product
                await Task.Delay(simulationClock.GetRealMillisecondsFromMinutes(10), cancellationToken);

                // Store the product and mark the product as in storage
                var simulatedTime = simulationClock.GetCurrentSimulatedTime();
                product.MarkAsInStorage(simulatedTime);
                order.UpdateStatus(OrderStatus.InStorage);
                location.Occupy(product.Id);

                await context.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Product {ProductId} stored at {LocationCode}",
                    product.Id, location.LocationCode);

                // Notify clients about the storage location update
                await hub.Clients.All.SendAsync("StorageLocationUpdated", new StorageLocationUpdate(
                    location.Id,
                    location.LocationCode,
                    location.Status.ToString(),
                    product.Id,
                    order.OrderNumber), cancellationToken);

                await Task.Delay(simulationClock.GetRealMillisecondsFromHours(1), cancellationToken);

                break;
            }
        }
    }
}