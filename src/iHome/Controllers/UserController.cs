using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnv;
    private readonly IUserManagementService _userManagementService;

    public UserController(IWebHostEnvironment hostingEnv, IUserManagementService userManagementService)
    {
        _hostingEnv = hostingEnv;
        _userManagementService = userManagementService;
    }

    [HttpGet("GetUsers/{searchPhrase}")]
    public async Task<IActionResult> GetUsers(string searchPhrase)
    {
        if (searchPhrase.Length < 3)
        {
            return Ok(Enumerable.Empty<User>());
        }

        var searchWildcard = $"*{searchPhrase}*";

        var filter = new UserFilter
        {
            Name = searchWildcard,
            Email = searchWildcard
        };

        if (_hostingEnv.IsDevelopment())
        {
            filter.Id = searchWildcard;
        }

        var users = await _userManagementService.GetUsers(new() { Filter = filter });

        return Ok(users.Users);
    }
}
