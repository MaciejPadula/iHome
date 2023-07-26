using iHome.Core.Models;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Logic.RoomDtoList;

public class RoomDtoBuilder : IRoomDtoBuilder
{
    public Task<RoomDto> Build(RoomModel room, User? user)
    {
        user ??= new User
        {
            Id = room.UserId
        };

        return Task.FromResult(new RoomDto(room, user));
    }
}
