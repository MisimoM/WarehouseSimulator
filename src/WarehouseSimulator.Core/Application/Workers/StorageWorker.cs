using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WarehouseSimulator.Core.Domain.Machines;
using WarehouseSimulator.Core.Domain.Machines.Events;
using WarehouseSimulator.Core.Domain.Orders;
using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.Domain.StorageLocations;
using WarehouseSimulator.Core.Domain.StorageLocations.Events;
using WarehouseSimulator.Core.Infrastructure.Belt;
using WarehouseSimulator.Core.Infrastructure.Persistence;
using WarehouseSimulator.Core.SignalR.Messages;

namespace WarehouseSimulator.Core.Application.Workers;

public class StorageWorker(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    BeltChannel belt,
    ISimulationClock simulationClock,
    IEventBus eventBus,
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

            location.Reserve();
            product.MarkAsBeingStored();
            await context.SaveChangesAsync(cancellationToken);

            await Task.Delay(simulationClock.GetRealMillisecondsFromMinutes(10), cancellationToken);

            var simulatedTime = simulationClock.GetCurrentSimulatedTime();
            location.Occupy(product.Id);
            product.MarkAsInStorage(simulatedTime);
            order.UpdateStatus(OrderStatus.InStorage);

            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Product {ProductId} stored at {LocationCode}",
                product.Id, location.LocationCode);

            await eventBus.PublishAsync(new StorageLocationUpdatedEvent(
                location.Id,
                location.Row,
                location.Column,
                location.Status,
                product.Id,
                order.OrderNumber,
                simulationClock.GetCurrentSimulatedTime()
            ));

            await Task.Delay(simulationClock.GetRealMillisecondsFromHours(1), cancellationToken);

            if (SimulationRandomizer.ShouldBreakDown())
            {
                var breakdownSimulatedTime = simulationClock.GetCurrentSimulatedTime();
                machine.Break(breakdownSimulatedTime);
                await context.SaveChangesAsync(cancellationToken);

                await eventBus.PublishAsync(new MachineBreakdownEvent(
                    machine.Id,
                    machine.Type,
                    machine.TotalBreakdowns
                ));

                logger.LogWarning("Storage machine broke down!");
            }
        }
    }
}