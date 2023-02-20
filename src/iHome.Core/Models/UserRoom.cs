using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHome.Core.Models;

[Table("UsersRooms", Schema = "maciejadmin")]
public class UserRoom
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid RoomId { get; set; }
    public required string UserId { get; set; }

    public virtual Room Room { get; set; }
}
