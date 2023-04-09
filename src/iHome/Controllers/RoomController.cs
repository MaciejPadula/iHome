using iHome.Core.Models;
using iHome.Core.Services.Rooms;
using iHome.Core.Services.Users;
using iHome.Devices.Contract.Interfaces;
using iHome.Devices.Contract.Models;
using iHome.Logic;
using iHome.Models.Requests;
using iHome.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
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
    public async Task<IActionResult> AddRoom([FromBody] AddRoomRequest request)
    {
        await _roomService.AddRoom(request.RoomName, _userAccessor.UserId);
        return Ok();
    }

    [HttpGet("GetRooms")]
    public async Task<IActionResult> GetRooms()
    {
        var rooms = (await _roomService.GetRooms(_userAccessor.UserId))
            .Select(room => new GetRoomsRoom
            {
                Id = room.Id,
                Name = room.Name,
                User = _userService.GetUserById(room.UserId).Result
                    ?? new User { Id = room.UserId, Name = string.Empty, Email = string.Empty }
            });
        
        return Ok(rooms);
    }

    [HttpGet("GetRoomUsers/{roomId}")]
    public async Task<IActionResult> GetRoomUsers(Guid roomId)
    {
        var users = (await _roomService.GetRoomUsers(roomId, _userAccessor.UserId))
            .Select(usr => _userService.GetUserById(usr.UserId).Result)
            .OfType<User>()
            .OrderBy(usr => usr.Name);

        return Ok(users);
    }

    [HttpPost("ShareRoom")]
    public async Task<IActionResult> ShareRoom([FromBody] ShareRoomRequest request)
    {
        await _roomService.ShareRoom(request.RoomId, request.UserId, _userAccessor.UserId);
        return Ok();
    }

    [HttpPost("UnshareRoom")]
    public async Task<IActionResult> ShareRoom([FromBody] UnshareRoomRequest request)
    {
        await _roomService.UnshareRoom(request.RoomId, request.UserId, _userAccessor.UserId);
        return Ok();
    }

    [HttpDelete("RemoveRoom/{roomId}")]
    public async Task<IActionResult> RemoveRoom(Guid roomId)
    {
        await _roomService.RemoveRoom(roomId, _userAccessor.UserId);
        return Ok();
    }
}
