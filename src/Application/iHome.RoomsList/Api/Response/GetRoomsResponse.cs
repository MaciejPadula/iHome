using iHome.Model;

namespace iHome.RoomsList.Api.Response;

public class GetRoomsResponse
{
    public required List<RoomDto> Rooms { get; set; }
}
