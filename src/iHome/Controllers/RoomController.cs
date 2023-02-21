using iHome.Core.Services.Rooms;
using iHome.Core.Services.Users;
using iHome.Logic;
using iHome.Models.Requests;
using iHome.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IUserService _userService;
    private readonly IUserAccessor _userAccessor;
    

    public RoomController(IRoomService roomService, IUserService userService, IUserAccessor userAccessor)
    {
        _roomService = roomService;
        _userService = userService;
        _userAccessor = userAccessor;
    }

    [HttpPost("AddRoom")]
    public IActionResult AddRoom([FromBody] AddRoomRequest request)
    {
        _roomService.AddRoom(request.RoomName, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetRooms")]
    public IActionResult GetRooms()
    {
        var devices = _roomService.GetRooms(_userAccessor.UserId)
            .Select(room => new GetRoomsRoom
            {
                Id = room.Id,
                Name = room.Name,
                UserId = room.UserId,
                UserEmail = _userService.GetUserById(room.UserId)?.Email ?? string.Empty
            });
        return Ok(devices);
    }

    [HttpDelete("RemoveRoom/{roomId}")]
    public IActionResult RemoveRoom(Guid roomId)
    {
        _roomService.RemoveRoom(roomId, _userAccessor.UserId);
        return Ok();
    }
}
