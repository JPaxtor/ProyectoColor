using Microsoft.AspNetCore.SignalR;

namespace MonitorUsuarios.Hubs
{
    public class AdminHub : Hub
    {
        public async Task BroadcastColor(int r, int g, int b, string colorName)
        {
            await Clients.All.SendAsync("ReceiveColorData", r, g, b, colorName);
        }
        public async Task SendMessageToClient(string conexionId, string message)
        {
            await Clients.Client(conexionId).SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendLogoutToClient(string conexionId)
        {
            await Clients.Client(conexionId).SendAsync("ReceiveLogout");
        }

        public async Task SendLogoutModule(string modulo)
        {
            await Clients.All.SendAsync("ReceiveLogout", modulo);
        }
    }
}