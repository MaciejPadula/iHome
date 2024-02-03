using iHome.Model;

namespace iHome.Suggestions.Api.Request;

public class GetSuggestedDevicesRequest
{
    public required string ScheduleName { get; set; }
    public required TimeModel ScheduleTime { get; set; }
}
