using iHome.Core.Services;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _userService = userService;
    }

    [HttpGet("GetUsers/{searchPhrase}")]
    public async Task<IActionResult> GetUsers(string searchPhrase)
    {
        var users = await _userService.GetUsers(searchPhrase);
        return Ok(users);
    }

    [HttpGet("GetRoomUsers/{roomId}")]
    public async Task<IActionResult> GetRoomUsers(Guid roomId)
    {
        var users = await _userService.GetRoomUsers(roomId, _userAccessor.UserId);
        return Ok(users);
    }
}
