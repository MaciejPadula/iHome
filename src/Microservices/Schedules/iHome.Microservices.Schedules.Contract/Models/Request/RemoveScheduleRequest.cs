using System;

namespace iHome.Microservices.Schedules.Contract.Models.Request
{
    public class RemoveScheduleRequest
    {
        public Guid ScheduleId { get; set; }
    }
}
