using Microsoft.AspNetCore.SignalR;

namespace iHome.Hubs
{
    public class RoomsHub: Hub
    {
        public async Task SendMessage(string uuid)
        {
            await Clients.All.SendAsync(uuid);
        }
    }
}
