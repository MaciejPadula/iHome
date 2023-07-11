using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models;

namespace iHome.Core.Services;

public interface IUserService
{
    Task<List<User>> GetUsers(string searchPhrase);
}

public class UserService : IUserService
{
    private readonly IUserManagementService _userManagementService;

    public UserService(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<List<User>> GetUsers(string searchPhrase)
    {
        if (searchPhrase.Length < 3)
        {
            return Enumerable.Empty<User>().ToList();
        }

        var searchWildcard = $"*{searchPhrase}*";

        var filter = new UserFilter
        {
            Name = searchWildcard,
            Email = searchWildcard
        };

        var users = await _userManagementService.GetUsers(new() { Filter = filter });

        return users?.Users?.ToList() ?? Enumerable.Empty<User>().ToList();
    }
}
