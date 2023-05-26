namespace iHome.Microservices.Schedules.Contract.Models.Request
{
    public class AddScheduleRequest
    {
        public string ScheduleName { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public string UserId { get; set; }
    }
}
