using System;

namespace iHome.Microservices.Widgets.Contract.Models.Request
{
    public class GetWidgetsRequest
    {
        public Guid RoomId { get; set; }
        public string UserId { get; set; }
    }
}
