using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.RoomsList.Features.GetUserRooms;

internal class GetUserRoomsQuery : IQuery<List<RoomDto>>
{
    public required string UserId { get; set; }
    public List<RoomDto> Result { get; set; } = default!;
}
