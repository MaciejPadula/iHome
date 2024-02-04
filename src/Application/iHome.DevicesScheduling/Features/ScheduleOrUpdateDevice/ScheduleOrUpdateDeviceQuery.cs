using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.DevicesScheduling.Features.ScheduleOrUpdateDevice;

public class ScheduleOrUpdateDeviceQuery : IQuery<Guid>
{
    public required Guid ScheduleId { get; set; }
    public required Guid DeviceId { get; set; }
    public required string DeviceData { get; set; }
    public required string UserId { get; set; }

    public Guid Result { get; set; }
}
