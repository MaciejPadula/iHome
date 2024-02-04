using iHome.Model;
using Web.Infrastructure.Cqrs.Mediator.Query;

namespace iHome.RoomSharing.Features.GetRoomUsers;

public class GetRoomUsersQuery : IQuery<List<string>>
{
    public Guid RoomId { get; set; }
    public string UserId { get; set; } = default!;
    public List<string> Result { get; set; } = default!;
}
