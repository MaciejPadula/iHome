using iHome.Models.Requests.Rooms;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Features.RoomsList.Api;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RoomsListController : BaseApiController
{
    private readonly IRoomsListService _roomsListService;

    public RoomsListController(IUserAccessor userAccessor, IRoomsListService roomsListService)
        : base(userAccessor)
    {
        _roomsListService = roomsListService;
    }

    [HttpPost("AddRoom")]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomRequest request)
    {
        await _roomsListService.AddRoom(request.RoomName, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetRooms")]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = await _roomsListService.GetUserRooms(_userAccessor.UserId);
        return Ok(rooms);
    }

    [HttpDelete("RemoveRoom/{roomId}")]
    public async Task<IActionResult> RemoveRoom(Guid roomId)
    {
        await _roomsListService.RemoveRoom(roomId, _userAccessor.UserId);
        return Ok();
    }
}
