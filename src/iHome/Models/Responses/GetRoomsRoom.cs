namespace iHome.Models.Responses;

public class GetRoomsRoom
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string UserId { get; init; }
    public required string UserEmail { get; set; }
}
