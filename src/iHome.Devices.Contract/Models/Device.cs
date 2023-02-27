using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Devices.Contract.Models;

[Table("Devices", Schema = "maciejadmin")]
public class Device
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DeviceType Type { get; init; }
    public string Data { get; set; } = string.Empty;
    public Guid HubId { get; set; }
    public Guid RoomId { get; set; }
    public string MacAddress { get; init; } = string.Empty;
}
