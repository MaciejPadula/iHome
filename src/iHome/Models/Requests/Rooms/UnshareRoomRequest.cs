namespace iHome.Models.Requests.Rooms;

public class UnshareRoomRequest
{
    public required Guid RoomId { get; set; }
    public required string UserId { get; set; }
}
