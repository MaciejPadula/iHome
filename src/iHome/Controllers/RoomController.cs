using iHome.Core.Services;
using iHome.Logic;
using iHome.Models.Requests.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RoomController : BaseApiController
{
    private readonly IRoomService _roomService;

    public RoomController(IUserAccessor userAccessor, IRoomService roomService)
        : base(userAccessor)
    {
        _roomService = roomService;
    }

    [HttpPost("AddRoom")]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomRequest request)
    {
        await _roomService.AddRoom(request.RoomName, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetRooms")]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _roomService.GetRooms(_userAccessor.UserId);
        return Ok(rooms);
    }

    [HttpDelete("RemoveRoom/{roomId}")]
    public async Task<IActionResult> RemoveRoom(Guid roomId)
    {
        await _roomService.RemoveRoom(roomId, _userAccessor.UserId);
        return Ok();
    }
}
