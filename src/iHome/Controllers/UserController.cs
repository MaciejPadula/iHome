using iHome.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("GetUsers/{searchPhrase}")]
    public async Task<IActionResult> GetUsers(string searchPhrase)
    {
        var users = await _userService.GetUsers(searchPhrase);
        return Ok(users);
    }
}
