using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WarehouseSimulator.Core.Domain.Orders;
using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.Infrastructure.Persistence;

namespace WarehouseSimulator.Core.Application.Workers;

public class OrderWorker(
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    ISimulationClock simulationClock,
    ILogger<OrderWorker> logger) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await using var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            var priority = SimulationRandomizer.GetOrderPriority();
            var simulatedTime = simulationClock.GetCurrentSimulatedTime();
            var order = Order.Create(priority, simulatedTime);

            await context.Orders.AddAsync(order, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Order {DisplayNumber} created with priority {Priority}",
                order.DisplayNumber, order.Priority);

            await Task.Delay(simulationClock.GetRealMillisecondsFromHours(2), cancellationToken);
        }
    }
}
