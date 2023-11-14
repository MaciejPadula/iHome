using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.RoomsList.Service.GetUserRooms;

internal class GetUserRoomsQuery : IQuery<List<RoomDto>>
{
    public string UserId { get; set; } = default!;
    public List<RoomDto> Result { get; set; } = default!;
}
