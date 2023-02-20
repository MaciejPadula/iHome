using iHome.Core.Models;

namespace iHome.Models;

public class GetRoomsRoom : Room
{
    public required string UserEmail { get; set; }
}
