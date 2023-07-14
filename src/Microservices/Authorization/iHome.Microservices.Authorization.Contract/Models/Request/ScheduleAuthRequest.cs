using System;

namespace iHome.Microservices.Authorization.Contract.Models.Request
{
    public class ScheduleAuthRequest
    {
        public Guid ScheduleId { get; set; }
        public string UserId { get; set; }
    }
}
