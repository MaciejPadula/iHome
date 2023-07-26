using iHome.Core.Models;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Logic.RoomDtoList;

public interface IRoomDtoBuilder
{
    Task<RoomDto> Build(RoomModel room, User? user);
}
