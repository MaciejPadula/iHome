using System;

namespace iHome.Microservices.Schedules.Contract.Models
{
    public class ScheduleDeviceModel
    {
        public Guid Id { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid DeviceId { get; set; }
        public string Name { get; set; }
        public string DeviceData { get; set; }
        public int Type { get; set; }
    }
}