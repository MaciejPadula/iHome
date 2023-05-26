using System;

namespace iHome.Microservices.Schedules.Contract.Models.Request
{
    public class GetScheduleRequest
    {
        public Guid ScheduleId { get; set; }
        public string UserId { get; set; }
    }
}
