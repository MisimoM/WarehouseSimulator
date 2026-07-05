using Microsoft.AspNetCore.SignalR;

namespace WarehouseSimulator.Api.SignalR
{
    public class WarehouseHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
