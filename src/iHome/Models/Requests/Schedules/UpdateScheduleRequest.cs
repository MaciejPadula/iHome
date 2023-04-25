namespace iHome.Models.Requests.Schedules;

public class UpdateScheduleRequest
{
    public required Guid ScheduleId { get; set; }
    public required int Day { get; set; }
    public required int Hour { get; set; }
    public required int Minute { get; set;}
}
