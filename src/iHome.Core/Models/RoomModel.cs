using iHome.Infrastructure.SQL.Models.RootTables;

namespace iHome.Core.Models;

public class RoomModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public User? User { get; set; }

    public RoomModel(Room room)
    {
        Id = room.Id;
        Name = room.Name;
    }
}
