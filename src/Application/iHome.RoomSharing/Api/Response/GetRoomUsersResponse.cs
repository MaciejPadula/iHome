using iHome.Model;

namespace iHome.RoomSharing.Api.Response;

public class GetRoomUsersResponse
{
    public required Dictionary<string, UserDto> Users { get; set; }
}
