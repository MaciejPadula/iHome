namespace iHome.Devices.Contract.Models;

public class Device
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required DeviceType Type { get; init; }
    public required string Data { get; set; }
    public required string HubId { get; set; }
    public required Guid RoomId { get; set; }
    public required string MacAddress { get; init; }
}
