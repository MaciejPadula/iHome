using System;

namespace iHome.Microservices.Widgets.Contract.Models.Request
{
    public class RemoveDeviceRequest
    {
        public Guid WidgetId { get; set; }
        public Guid DeviceId { get; set; }
        public string UserId { get; set; }
    }
}
