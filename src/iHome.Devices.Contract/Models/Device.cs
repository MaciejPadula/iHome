using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Devices.Contract.Models;

[Table("Devices", Schema = "maciejadmin")]
public class Device
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required DeviceType Type { get; init; }
    public required string Data { get; set; }
    public required Guid HubId { get; set; }
    public required Guid RoomId { get; set; }
    public required string MacAddress { get; init; }
}
