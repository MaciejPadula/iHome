namespace iHome.Models.Requests.Suggestion;

public class GetSuggestedDevicesRequest
{
    public required string ScheduleName { get; set; }
    public required string ScheduleTime { get; set; }
}
