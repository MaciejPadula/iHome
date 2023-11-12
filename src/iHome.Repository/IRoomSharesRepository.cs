namespace iHome.Repository;

public interface IRoomSharesRepository
{
    Task Add(Guid roomId, string userId);
    Task<IEnumerable<string>> GetRoomUsersIds(Guid roomId);
    Task Remove(Guid roomId, string userId);
}
