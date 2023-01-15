namespace iHome.Core.Models;

public class Room
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }

    public required Guid UserId { get; init; }
}
