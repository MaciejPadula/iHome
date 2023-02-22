using iHome.Core.Models;

namespace iHome.Core.Services.Users;

public interface IUserService
{
    IEnumerable<User> GetUsers(UserFilter userFilter);
    User? GetUserById(string userId);
    bool UserExist(UserFilter userFilter);
}
