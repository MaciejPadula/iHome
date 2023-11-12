using iHome.Infrastructure.Mappers;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models;
using iHome.Model;
using iHome.Repository;

namespace iHome.Infrastructure.Repository;

internal class UserRepository : IUserRepository
{
    private readonly IUserManagementService _userManagementService;

    public UserRepository(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<UserDto?> GetUser(string userId)
    {
        var response = await _userManagementService.GetUserById(new()
        {
            UserId = userId
        });

        return response?.User?.ToDto();
    }

    public async Task<Dictionary<string, UserDto>> GetUsers(IEnumerable<string> userIds)
    {
        var response = await _userManagementService.GetUsersByIds(new()
        {
            Ids = userIds
        });

        return response?
            .Users?
            .Select(u => new KeyValuePair<string, UserDto>(u.Key, u.Value.ToDto()))?
            .ToDictionary(u => u.Key, u => u.Value) ?? new Dictionary<string, UserDto>();
    }
}
