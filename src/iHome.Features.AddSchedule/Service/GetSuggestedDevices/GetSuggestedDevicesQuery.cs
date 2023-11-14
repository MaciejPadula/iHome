using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.AddSchedule.Service.GetSuggestedDevices;

internal class GetSuggestedDevicesQuery : IQuery<List<DeviceDto>>
{
    public required string Name { get; set; }
    public required string Time { get; set; }
    public required string UserId { get; set; }
    public List<DeviceDto> Result { get; set; } = default!;
}
