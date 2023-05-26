using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models;
using iHome.Microservices.UsersApi.Contract.Models.Request;
using iHome.Microservices.UsersApi.Contract.Models.Response;
using iHome.Microservices.UsersApi.Infrastructure.Helpers;
using iHome.Microservices.UsersApi.Infrastructure.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;

namespace iHome.Core.Services.Users;

public class Auth0UserManagementService : IUserManagementService
{
    private readonly HttpClient _client;

    public Auth0UserManagementService(HttpClient client, IOptions<Auth0ApiConfiguration> options)
    {
        _client = client;
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.ApiToken);
    }

    public async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request)
    {
        var users = await GetUsers(new()
        {
            Filter = new UserFilter { Id = request.UserId }
        });
        return new() 
        { 
            User = users.Users.FirstOrDefault()
        };
    }

    public async Task<GetUsersResponse> GetUsers(GetUsersRequest request)
    {
        var filter = request.Filter;
        var url = new Auth0QueryBuilder("https://dev-e7eyj4xg.eu.auth0.com")
            .WithUserId(filter.Id)
            .WithEmail(filter.Email)
            .WithUsername(filter.Name)
            .Build();

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

        return new() 
        { 
            Users = response
                .Select(u => new User
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                })
        };
    }

    public async Task<UserExistResponse> UserExist(UserExistRequest request)
    {
        var users = await GetUsers(new()
        {
            Filter = request.Filter
        });
        return new()
        {
            Exists = users.Users.Any()
        };
    }
}
