namespace iHome.Models.Requests;

public class ShareRoomRequest
{
    public required Guid RoomId { get; set; }
    public required string UserId { get; set; }
}
