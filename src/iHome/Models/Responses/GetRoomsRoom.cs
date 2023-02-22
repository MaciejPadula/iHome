using iHome.Core.Models;

namespace iHome.Models.Responses;

public class GetRoomsRoom
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required User User { get; set; }
}
