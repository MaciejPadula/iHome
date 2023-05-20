using System;

namespace iHome.Microservices.Widgets.Contract.Models.Request
{
    public class RemoveWidgetRequest
    {
        public Guid WidgetId { get; set; }
        public string UserId { get; set; }
    }
}
