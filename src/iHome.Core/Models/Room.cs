using iHome.Devices.Contract.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models;

[Table("Rooms", Schema = "maciejadmin")]
public class Room
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; init; }

    public required Guid UserId { get; init; }
}
