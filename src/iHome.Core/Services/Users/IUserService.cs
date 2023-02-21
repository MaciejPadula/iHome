using iHome.Core.Models;

namespace iHome.Core.Services.Users;

public interface IUserService
{
    IEnumerable<User> GetUsers(string pattern);
    User? GetUserById(string userId);
    bool UserExist(UserFilter userFilter);
}
