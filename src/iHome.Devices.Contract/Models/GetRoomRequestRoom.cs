namespace iHome.Devices.Contract.Models;

public class GetRoomRequestRoom
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
}
