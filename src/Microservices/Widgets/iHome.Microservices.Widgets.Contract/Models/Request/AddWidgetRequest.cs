using System;

namespace iHome.Microservices.Widgets.Contract.Models.Request
{
    public class AddWidgetRequest
    {
        public WidgetType Type { get; set; }
        public Guid RoomId { get; set; }
        public bool ShowBorder { get; set; }
        public string UserId { get; set; }
    }
}
