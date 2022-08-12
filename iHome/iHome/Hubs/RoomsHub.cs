using Microsoft.AspNetCore.SignalR;

namespace iHome.Hubs
{
    public class RoomsHub: Hub
    {
        public async Task SendMessage(string input)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "updateView");
        }
    }
}
