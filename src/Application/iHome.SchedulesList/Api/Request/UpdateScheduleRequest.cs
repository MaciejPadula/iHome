namespace iHome.SchedulesList.Api.Request;

public class UpdateScheduleRequest
{
    public required Guid ScheduleId { get; set; }
    public required int Day { get; set; }
    public required string ScheduleTime { get; set; }
}
