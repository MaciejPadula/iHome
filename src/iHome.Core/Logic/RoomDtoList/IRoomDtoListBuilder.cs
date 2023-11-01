using iHome.Core.Models;
using iHome.Microservices.RoomsManagement.Contract.Models;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Logic.RoomDtoList;

public interface IRoomDtoListBuilder
{
    Task<List<RoomDto>> Build(List<RoomModel> rooms, Dictionary<string, User> users);
}
