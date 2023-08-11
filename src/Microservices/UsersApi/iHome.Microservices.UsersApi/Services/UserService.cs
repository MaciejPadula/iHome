using iHome.Microservices.UsersApi.Contract.Models;
using iHome.Microservices.UsersApi.Contract.Models.Request;
using iHome.Microservices.UsersApi.Contract.Models.Response;
using iHome.Microservices.UsersApi.Infrastructure.Repositories;

namespace iHome.Microservices.UsersApi.Services;

public interface IUserService
{
    Task<GetUsersResponse> GetFilteredUsers(GetUsersRequest request);
    Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request);
    Task<UserExistResponse> UserExist(UserExistRequest request);
    Task<GetUsersByIdsResponse> GetUsersByIds(GetUsersByIdsRequest request);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetUsersResponse> GetFilteredUsers(GetUsersRequest request)
    {
        var filters = new List<UserFilter> { request.Filter };

        return new()
        {
            Users = await _userRepository.GetFilteredUsers(filters)
        };
    }

    public async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request)
    {
        return new()
        {
            User = await _userRepository.GetUserById(request.UserId)
        };
    }

    public async Task<GetUsersByIdsResponse> GetUsersByIds(GetUsersByIdsRequest request)
    {
        var ids = request.Ids.ToList();

        if (!ids.Any()) 
        {
            return new() { Users = new Dictionary<string, User>() };
        }

        return new()
        {
            Users = await _userRepository.GetUsersByIds(ids)
        };
    }

    public async Task<UserExistResponse> UserExist(UserExistRequest request)
    {
        return new()
        {
            Exists = await _userRepository.CheckUserExistence(request.Filter)
        };
    }
}