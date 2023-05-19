using iHome.Core.Models;
using System.Collections.Generic;

namespace iHome.Core.Repositories.Rooms;

public interface IUserRoomRepository
{
    Task AddUserRoom(Guid roomId, string userId);
    Task RemoveUserRoom(Guid roomId, string userId);

    Task<List<string>> GetRoomUsersIds(Guid roomId);
}
