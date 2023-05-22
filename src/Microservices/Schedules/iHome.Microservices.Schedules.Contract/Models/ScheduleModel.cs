using System;

namespace iHome.Microservices.Schedules.Contract.Models
{
    public class ScheduleModel
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool Runned { get; set; }
        public string UserId { get; set; }
    }
}