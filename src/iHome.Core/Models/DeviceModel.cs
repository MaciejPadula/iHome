using iHome.Infrastructure.SQL.Models;

namespace iHome.Core.Models;

public class DeviceModel
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public DeviceType Type { get; init; }
    public Guid RoomId { get; set; }
    public string MacAddress { get; init; }
    public string? Data { get; set; }

    public DeviceModel(Device? device)
    {
        Id = device?.Id ?? Guid.Empty;
        Name = device?.Name ?? string.Empty;
        Type = device?.Type ?? DeviceType.Unknown;
        RoomId = device?.RoomId ?? Guid.Empty;
        MacAddress = device?.MacAddress ?? string.Empty;
    }
}
