using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Microservices.UsersApi.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserById(string userId);
    Task<Dictionary<string, User>> GetUsersByIds(IEnumerable<string> userIds);
    Task<List<User>> GetFilteredUsers(IEnumerable<UserFilter> filters);
    Task<bool> CheckUserExistence(UserFilter filter);
}
