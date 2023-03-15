using iHome.Core.Models;

namespace iHome.Core.Services.Users;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers(UserFilter userFilter);
    Task<User?> GetUserById(string userId);
    Task<bool> UserExist(UserFilter userFilter);
}
