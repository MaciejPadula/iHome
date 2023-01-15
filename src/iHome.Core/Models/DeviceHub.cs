namespace iHome.Core.Models;

public class DeviceHub
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }

    public required Guid RoomId { get; init; }
}
