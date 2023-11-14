namespace iHome.Features.AddSchedule.Api.Request;

public class AddScheduleRequest
{
    public required string ScheduleName { get; set; }
    public required int Day { get; set; }
    public required string ScheduleTime { get; set; }
}
