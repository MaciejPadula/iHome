using iHome.Microservices.OpenAI.Contract.Models;
using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Suggestions.Features.GetSuggestedDevices;

public class GetSuggestedDevicesQuery : IQuery<List<Guid>>
{
    public required string Name { get; set; }
    public required TimeModel Time { get; set; }
    public required IEnumerable<DeviceDto> Devices { get; set; }
    public List<Guid> Result { get; set; } = default!;
}
