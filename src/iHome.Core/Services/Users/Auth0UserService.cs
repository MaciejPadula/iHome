using iHome.Core.Helpers;
using iHome.Core.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace iHome.Core.Services.Users;

public class Auth0UserService : IUserService
{
    private readonly HttpClient _client;

    public Auth0UserService(HttpClient client, Auth0ApiConfiguration apiConfiguration)
    {
        _client = client;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiConfiguration.Token);
    }

    public async Task<User?> GetUserById(string userId)
    {
        var users = await GetUsers(new UserFilter { Id = userId });
        return users.FirstOrDefault();
    }

    public async Task<IEnumerable<User>> GetUsers(UserFilter userFilter)
    {
        var url = new Auth0QueryBuilder("https://dev-e7eyj4xg.eu.auth0.com")
            .WithUserId(userFilter.Id)
            .WithEmail(userFilter.Email)
            .WithUsername(userFilter.Name)
            .Build();

        try
        {
            var users = await _client.GetAsync(url);
            return JsonConvert.DeserializeObject<IEnumerable<User>>(await users.Content.ReadAsStringAsync()) ?? Enumerable.Empty<User>();
        }
        catch (JsonSerializationException)
        {
            return Enumerable.Empty<User>();
        }
    }

    public async Task<bool> UserExist(UserFilter userFilter)
    {
        var users = await GetUsers(userFilter);
        return users.Any();
    }
}
