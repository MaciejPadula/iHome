using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.RoomsList.Service.GetUserRooms;

internal class GetUserRoomsQuery : IQuery<IEnumerable<RoomDto>>
{
    public string UserId { get; set; }
    public IEnumerable<RoomDto> Result { get; set; }
}
