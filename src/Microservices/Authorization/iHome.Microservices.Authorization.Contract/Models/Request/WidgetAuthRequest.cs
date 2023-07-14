using System;

namespace iHome.Microservices.Authorization.Contract.Models.Request
{
    public class WidgetAuthRequest
    {
        public Guid WidgetId { get; set; }
        public string UserId { get; set; }
    }
}
