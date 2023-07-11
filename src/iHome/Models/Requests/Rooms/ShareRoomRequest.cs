namespace iHome.Models.Requests.Rooms;

public class ShareRoomRequest
{
    public required Guid RoomId { get; set; }
    public required string UserId { get; set; }
}
