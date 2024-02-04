using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.DevicesScheduling.Features.GetScheduleDevices;

public class GetScheduleDevicesQuery : IQuery<List<ScheduleDeviceDto>>
{
    public required string UserId { get; set; }
    public required Guid ScheduleId { get; set; }
    public List<ScheduleDeviceDto> Result { get; set; } = default!;
}
