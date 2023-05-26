using System;

namespace iHome.Microservices.Schedules.Contract.Models.Request
{
    public class AddOrUpdateDeviceScheduleRequest
    {
        public Guid ScheduleId { get; set; }
        public Guid DeviceId { get; set; }
        public string DeviceData { get; set; }
        public string UserId { get; set; }
    }
}
