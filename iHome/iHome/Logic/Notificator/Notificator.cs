using iHome.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace iHome.Logic.Notificator
{
    public class Notificator : INotificator
    {
        private readonly IHubContext<RoomsHub> _hubContext;

        public Notificator(IHubContext<RoomsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void NotifyUsers(List<string> uuids)
        {
            uuids.ForEach(uuid => NotifyUser(uuid));
        }

        public void NotifyUser(string uuid)
        {
            _hubContext.Clients.Group(uuid).SendAsync("ReceiveMessage", "updateView");
        }
    }
}
