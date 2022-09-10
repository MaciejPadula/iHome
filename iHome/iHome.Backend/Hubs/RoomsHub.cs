using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;

namespace iHome.Hubs
{
    public class RoomsHub: Hub
    {
        public async Task LoginToSignalR(string uuid)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, uuid);
        }
        public async Task SendMessage(string input)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "updateView");
        }
    }
}
