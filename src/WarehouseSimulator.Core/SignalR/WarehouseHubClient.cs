using Microsoft.AspNetCore.SignalR.Client;

namespace WarehouseSimulator.Core.SignalR;

public class WarehouseHubClient : IAsyncDisposable
{
    private readonly HubConnection _hubConnection;

    public WarehouseHubClient()
    {
        // Fixa config
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7286/warehouse-hub")
            .Build();
    }

    public async Task StartAsync()
    {
        if (_hubConnection.State == HubConnectionState.Disconnected)
            await _hubConnection.StartAsync();
    }

    public void On<T>(string eventName, Func<T, Task> handler)
    {
        _hubConnection.On(eventName, handler);
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }
}
