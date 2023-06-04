using System;

namespace iHome.Microservices.Schedules.Contract.Models.Request
{
    public class RemoveDeviceScheduleRequest
    {
        public Guid ScheduleId { get; set; }
        public Guid DeviceId { get; set; }
    }
}
