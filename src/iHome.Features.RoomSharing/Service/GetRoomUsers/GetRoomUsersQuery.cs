using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.Features.RoomSharing.Service.GetRoomUsers;

internal class GetRoomUsersQuery : IQuery<IEnumerable<UserDto>>
{
    public Guid RoomId { get; set; }
    public string UserId { get; set; } = default!;
    public IEnumerable<UserDto> Result { get; set; } = default!;
}
