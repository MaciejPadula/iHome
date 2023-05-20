using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iHome.Infrastructure.SQL.Models.Enums;

namespace iHome.Infrastructure.SQL.Models.RootTables;

[Table("Devices", Schema = "maciejadmin")]
public class Device
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; } = string.Empty;
    public required int Type { get; init; }
    public required Guid RoomId { get; set; }
    public required string MacAddress { get; init; } = string.Empty;

    public virtual Room? Room { get; set; }
}
