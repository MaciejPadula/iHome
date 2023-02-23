namespace iHome.Models.Requests;

public class UnshareRoomRequest
{
    public required Guid RoomId { get; set; }
    public required string UserId { get; set; }
}
