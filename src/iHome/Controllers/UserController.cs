using iHome.Core.Models;
using iHome.Core.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnv;
    private readonly IUserService _userService;

    public UserController(IWebHostEnvironment hostingEnv, IUserService userService)
    {
        _hostingEnv = hostingEnv;
        _userService = userService;
    }

    [HttpGet("GetUsers/{searchPhrase}")]
    public IActionResult GetUsers(string searchPhrase)
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

        var users = _userService.GetUsers(filter);

        return Ok(users);
    }
}
