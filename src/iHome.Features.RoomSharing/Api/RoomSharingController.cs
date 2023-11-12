using iHome.Features.RoomSharing.Api.Request;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Features.RoomSharing.Api;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RoomSharingController : BaseApiController
{
    private readonly IRoomSharingService _roomSharingService;

    public RoomSharingController(IRoomSharingService roomSharingService, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _roomSharingService = roomSharingService;
    }

    [HttpPost("ShareRoom")]
    public async Task<IActionResult> ShareRoom([FromBody] ShareRoomRequest request)
    {
        await _roomSharingService.ShareRoom(request.RoomId, request.UserId, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("UnshareRoom")]
    public async Task<IActionResult> UnshareRoom([FromBody] UnshareRoomRequest request)
    {
        await _roomSharingService.UnshareRoom(request.RoomId, request.UserId, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetRoomUsers/{roomId}")]
    public async Task<IActionResult> GetRoomUsers(Guid roomId)
    {
        var users = await _roomSharingService.GetRoomUsers(roomId, _userAccessor.UserId);
        return Ok(users);
    }
}
