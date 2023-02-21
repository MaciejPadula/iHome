using iHome.Core.Models;

namespace iHome.Core.Services.Users;

public class Auth0UserService : IUserService
{
    public User? GetUserById(string userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetUsers(string pattern)
    {
        throw new NotImplementedException();
    }

    public bool UserExist(UserFilter userFilter)
    {
        throw new NotImplementedException();
    }
}
