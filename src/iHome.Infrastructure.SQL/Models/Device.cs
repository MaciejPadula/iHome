using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Infrastructure.SQL.Models;

[Table("Devices", Schema = "maciejadmin")]
public class Device
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    public required DeviceType Type { get; init; }
    public required string Data { get; set; } = string.Empty;
    public required Guid RoomId { get; set; }
    public required string MacAddress { get; init; } = string.Empty;

    public virtual Room? Room { get; set; }
}
