using System;

namespace iHome.Microservices.Schedules.Contract.Models.Request
{
    public class GetScheduleDevicesRequest
    {
        public Guid ScheduleId { get; set; }
    }
}
