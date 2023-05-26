namespace iHome.Microservices.RoomsManagement.Infrastructure.Repositories;

public interface IUserRoomRepository
{
    Task AddUserRoom(Guid roomId, string userId);
    Task RemoveUserRoom(Guid roomId, string userId);

    Task<IEnumerable<string>> GetRoomUsersIds(Guid roomId);
}
