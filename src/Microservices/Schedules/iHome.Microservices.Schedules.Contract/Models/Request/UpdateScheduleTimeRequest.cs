using System;

namespace iHome.Microservices.Schedules.Contract.Models.Request
{
    public class UpdateScheduleTimeRequest
    {
        public Guid ScheduleId { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }
}
