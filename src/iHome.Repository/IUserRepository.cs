using iHome.Model;

namespace iHome.Repository;

public interface IUserRepository
{
    Task<UserDto?> GetUser(string userId);
    Task<Dictionary<string, UserDto>> GetUsers(IEnumerable<string> userIds);
}
