using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iHome.Infrastructure.SQL.Models.RootTables;

namespace iHome.Infrastructure.SQL.Models.ConnectionTables;

[Table("UsersRooms", Schema = "maciejadmin")]
public class UserRoom
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid RoomId { get; set; }
    public required string UserId { get; set; }

    public virtual Room? Room { get; set; }
}
