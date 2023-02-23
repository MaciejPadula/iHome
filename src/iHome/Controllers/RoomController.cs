using iHome.Core.Models;
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
                User = _userService.GetUserById(room.UserId)
                    ?? new User { Id = room.UserId, Name = string.Empty, Email = string.Empty }
            });
        return Ok(devices);
    }

    [HttpGet("GetRoomUsers/{roomId}")]
    public IActionResult GetRoomUsers(Guid roomId)
    {
        var x = _roomService.GetRoomUsers(roomId, _userAccessor.UserId)
            .Select(usr => _userService.GetUserById(usr.UserId))
            .OfType<User>()
            .OrderBy(usr => usr.Name);

        return Ok(x);
    }

    [HttpPost("ShareRoom")]
    public IActionResult ShareRoom([FromBody] ShareRoomRequest request)
    {
        _roomService.ShareRoom(request.RoomId, request.UserId, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("UnshareRoom")]
    public IActionResult ShareRoom([FromBody] UnshareRoomRequest request)
    {
        _roomService.UnshareRoom(request.RoomId, request.UserId, _userAccessor.UserId);
        return Ok();
    }

    [HttpDelete("RemoveRoom/{roomId}")]
    public IActionResult RemoveRoom(Guid roomId)
    {
        _roomService.RemoveRoom(roomId, _userAccessor.UserId);
        return Ok();
    }
}
