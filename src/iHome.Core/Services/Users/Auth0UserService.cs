using iHome.Core.Helpers;
using iHome.Core.Models;
using iHome.Shared.Logic;
using System.Net.Http.Headers;

namespace iHome.Core.Services.Users;

public class Auth0UserService : IUserService
{
    private readonly JsonHttpClient _client;

    public Auth0UserService(JsonHttpClient client, Auth0ApiConfiguration apiConfiguration)
    {
        _client = client;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiConfiguration.Token);
    }

    public User? GetUserById(string userId)
    {
        return GetUsers(new UserFilter { Id = userId }).FirstOrDefault();
    }

    public IEnumerable<User> GetUsers(UserFilter userFilter)
    {
        var url = new Auth0QueryBuilder("https://dev-e7eyj4xg.eu.auth0.com")
            .WithUserId(userFilter.Id)
            .WithEmail(userFilter.Email)
            .Build();
        var users = _client.GetSync<IEnumerable<User>>(url);

        return users ?? Enumerable.Empty<User>();
    }

    public bool UserExist(UserFilter userFilter)
    {
        return GetUsers(userFilter).Any();
    }
}
