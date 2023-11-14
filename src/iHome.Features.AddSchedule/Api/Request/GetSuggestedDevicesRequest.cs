namespace iHome.Features.AddSchedule.Api.Request;

public class GetSuggestedDevicesRequest
{
    public required string ScheduleName { get; set; }
    public required string ScheduleTime { get; set; }
}
