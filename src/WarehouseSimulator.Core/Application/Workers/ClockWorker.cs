using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using WarehouseSimulator.Core.Domain.Shared;
using WarehouseSimulator.Core.SignalR;
using WarehouseSimulator.Core.SignalR.Messages;

namespace WarehouseSimulator.Core.Application.Workers;

public class ClockWorker(
    ISimulationClock simulationClock,
    IHubContext<WarehouseHub> hub) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await hub.Clients.All.SendAsync("ClockUpdated",
                new ClockUpdate(simulationClock.GetCurrentSimulatedTime()),
                cancellationToken
            );

            await Task.Delay(5000, cancellationToken);
        }
    }
}
