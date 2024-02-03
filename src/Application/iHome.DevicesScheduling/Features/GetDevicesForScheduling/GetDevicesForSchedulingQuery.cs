using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.DevicesScheduling.Features.GetDevicesForScheduling;

internal class GetDevicesForSchedulingQuery : IQuery<List<DeviceDto>>
{
    public required string UserId { get; set; }
    public List<DeviceDto> Result { get; set; } = default!;
}
