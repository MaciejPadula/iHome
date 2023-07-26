using iHome.Microservices.UsersApi.Contract.Models;
using iHome.Microservices.UsersApi.Infrastructure.Helpers;
using iHome.Microservices.UsersApi.Infrastructure.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace iHome.Microservices.UsersApi.Infrastructure.Repositories;

public class Auth0UserRepository : IUserRepository
{
    private readonly HttpClient _client;
    private readonly Auth0ApiConfiguration _configuration;

    public Auth0UserRepository(HttpClient client, Auth0ApiConfiguration options)
    {
        _client = client;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.ApiToken);
        _configuration = options;
    }

    public async Task<bool> CheckUserExistence(UserFilter filter)
    {
        var users = await GetFilteredUsers(new List<UserFilter> { filter });

        return users?.Any() ?? false;
    }

    public async Task<List<User>> GetFilteredUsers(IEnumerable<UserFilter> filters)
    {
        var builder = new Auth0QueryBuilder(_configuration.ApiUrl);

        foreach (var filter in filters) 
        {
            builder
                .WithUserId(filter.Id)
                .WithEmail(filter.Email)
                .WithUsername(filter.Name);
        }

        var url = builder.Build();

        IEnumerable<UserResponse> response;
        try
        {
            var users = await _client.GetAsync(url);
            var res = await users.Content.ReadAsStringAsync();
            response = JsonSerializer.Deserialize<IEnumerable<UserResponse>>(res) ?? Enumerable.Empty<UserResponse>();
        }
        catch (JsonException)
        {
            response = Enumerable.Empty<UserResponse>();
        }

        return response
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                })
                .ToList();
    }

    public async Task<User?> GetUserById(string userId)
    {
        var filter = new UserFilter
        {
            Id = userId
        };

        var users = await GetFilteredUsers(new List<UserFilter> { filter });

        return users?.FirstOrDefault();
    }

    public async Task<Dictionary<string, User>> GetUsersByIds(IEnumerable<string> userIds)
    {
        var filters = userIds
            .Select(id => new UserFilter
            {
                Id = id
            });

        var users = await GetFilteredUsers(filters);

        return users
            .ToDictionary(u => u.Id, u => u);
    }
}
