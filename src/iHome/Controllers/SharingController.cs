using iHome.Core.Services;
using iHome.Logic;
using iHome.Models.Requests.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SharingController : BaseApiController
{
    private readonly ISharingService _sharingService;

    public SharingController(ISharingService sharingService, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _sharingService = sharingService;
    }

    [HttpPost("ShareRoom")]
    public async Task<IActionResult> ShareRoom([FromBody] ShareRoomRequest request)
    {
        await _sharingService.ShareRoom(request.RoomId, request.UserId, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("UnshareRoom")]
    public async Task<IActionResult> UnshareRoom([FromBody] UnshareRoomRequest request)
    {
        await _sharingService.UnshareRoom(request.RoomId, request.UserId, _userAccessor.UserId);
        return Ok();
    }
}
