using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WarehouseSimulator.Core.Domain.Machines;
using WarehouseSimulator.Core.Domain.Machines.Events;
using WarehouseSimulator.Core.Domain.Orders;
using WarehouseSimulator.Core.Domain.Products;
using WarehouseSimulator.Core.Domain.Products.Events;
using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.Domain.Shared.Events;
using WarehouseSimulator.Core.Infrastructure.Belt;
using WarehouseSimulator.Core.Infrastructure.Persistence;

namespace WarehouseSimulator.Core.Application.Workers;

public class ProductionWorker(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    BeltChannel belt,
    ISimulationClock simulationClock,
    IEventBus eventBus,
    ILogger<ProductionWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var machine = await context.Machines
                .FirstAsync(m => m.Type == MachineType.Production, cancellationToken);

            if (machine.Status is not MachineStatus.Running)
            {
                logger.LogWarning("Production machine is not running, waiting...");
                await simulationClock.Delay(TimeSpan.FromHours(1), cancellationToken);
                continue;
            }

            var order = await context.Orders
                .Where(o => o.Status == OrderStatus.Pending)
                .OrderByDescending(o => o.Priority == Priority.Express)
                .ThenBy(o => o.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (order is null)
            {
                logger.LogInformation("No pending orders, waiting...");
                await simulationClock.Delay(TimeSpan.FromHours(1), cancellationToken);
                continue;
            }

            if (belt.Count >= 10)
            {
                logger.LogWarning("Belt is full, waiting...");
                await simulationClock.Delay(TimeSpan.FromHours(1), cancellationToken);
                continue;
            }

            var simulatedTime = simulationClock.GetCurrentSimulatedTime();
            var product = Product.Create(order, simulatedTime);

            order.UpdateStatus(OrderStatus.InProduction);

            context.Products.Add(product);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Product created for order {DisplayNumber}", order.DisplayNumber);
            
            await belt.Writer.WriteAsync(product, cancellationToken);

            logger.LogInformation("Product placed on belt for order {DisplayNumber}", order.DisplayNumber);

            await eventBus.PublishAsync(new ProductPlacedOnBeltEvent(
                product.Id,
                order.OrderNumber,
                product.Priority
            ));

            await simulationClock.Delay(TimeSpan.FromMinutes(20), cancellationToken);

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
                
                logger.LogWarning("Production machine broke down!");
            }
        }
    }
}