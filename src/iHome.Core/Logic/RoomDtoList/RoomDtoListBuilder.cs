using iHome.Core.Models;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Logic.RoomDtoList;

public class RoomDtoListBuilder : IRoomDtoListBuilder
{
    private readonly IRoomDtoBuilder _roomDtoBuilder;

    public RoomDtoListBuilder(IRoomDtoBuilder roomDtoBuilder)
    {
        _roomDtoBuilder = roomDtoBuilder;
    }

    public async Task<List<RoomDto>> Build(List<RoomModel> rooms, Dictionary<string, User> users)
    {
        var tasks = rooms.Select(async room =>
        {
            users.TryGetValue(room.UserId, out var user);
            return await _roomDtoBuilder.Build(room, user);
        }).ToList();

        await Task.WhenAll(tasks);

        return tasks.Select(t => t.Result).ToList();
    }
}
