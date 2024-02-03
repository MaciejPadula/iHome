using iHome.Model;

namespace iHome.SchedulesList.Api.Response;

public class GetSchedulesResponse
{
    public required List<ScheduleDto> Schedules { get; set; }
}
