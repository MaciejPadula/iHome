namespace iHome.Features.RoomSharing.Api.Request;

public class UnshareRoomRequest
{
    public required Guid RoomId { get; set; }
    public required string UserId { get; set; }
}
