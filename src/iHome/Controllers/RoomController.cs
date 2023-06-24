using iHome.Core.Models;
using iHome.Logic;
using iHome.Microservices.RoomsManagement.Contract;
using iHome.Microservices.UsersApi.Contract;
using iHome.Microservices.UsersApi.Contract.Models;
using iHome.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class RoomController : ControllerBase
{
    private readonly IRoomManagementService _roomManagementService;
    private readonly IRoomSharingService _roomSharingService;
    private readonly IUserManagementService _userManagementService;
    private readonly IUserAccessor _userAccessor;

    public RoomController(IRoomManagementService roomManagementService, IUserAccessor userAccessor, IUserManagementService userManagementService, IRoomSharingService roomSharingService)
    {
        _roomManagementService = roomManagementService;
        _userAccessor = userAccessor;
        _userManagementService = userManagementService;
        _roomSharingService = roomSharingService;
    }

    [HttpPost("AddRoom")]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomRequest request)
    {
        await _roomManagementService.AddRoom(new()
        {
            RoomName = request.RoomName,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpGet("GetRooms")]
    public async Task<IActionResult> GetRooms()
    {
        var response = (await _roomManagementService.GetRooms(new()
        {
            UserId = _userAccessor.UserId
        }));

        var rooms = response?.Rooms?.Select(r => new RoomDTO(r))?.ToList() ?? Enumerable.Empty<RoomDTO>();

        foreach (var room in rooms)
        {
            var usr = await _userManagementService.GetUserById(new() { UserId = room.User.Id });
            if (usr?.User == null)
            {
                continue;
            }

            room.User = usr.User;
        }

        return Ok(rooms);
    }

    [HttpGet("GetRoomUsers/{roomId}")]
    public async Task<IActionResult> GetRoomUsers(Guid roomId)
    {
        var userIds = await _roomSharingService.GetRoomUserIds(new()
        {
            RoomId = roomId,
            UserId = _userAccessor.UserId
        });

        var users = new List<User>();

        foreach (var uid in userIds.UsersIds)
        {
            var usr = await _userManagementService.GetUserById(new() { UserId = uid });
            if (usr?.User == null)
            {
                continue;
            }
            users.Add(usr.User);
        }

        return Ok(users.OrderBy(u => u.Name));
    }

    [HttpPost("ShareRoom")]
    public async Task<IActionResult> ShareRoom([FromBody] ShareRoomRequest request)
    {
        await _roomSharingService.ShareRoomToUser(new()
        {
            RoomId = request.RoomId,
            CallerUserId = _userAccessor.UserId,
            SubjectUserId = request.UserId
        });
        return Ok();
    }

    [HttpPost("UnshareRoom")]
    public async Task<IActionResult> ShareRoom([FromBody] UnshareRoomRequest request)
    {
        await _roomSharingService.UnshareRoomFromUser(new()
        {
            RoomId = request.RoomId,
            CallerUserId = _userAccessor.UserId,
            SubjectUserId = request.UserId
        });
        return Ok();
    }

    [HttpDelete("RemoveRoom/{roomId}")]
    public async Task<IActionResult> RemoveRoom(Guid roomId)
    {
        await _roomManagementService.RemoveRoom(new()
        {
            RoomId = roomId,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }
}
