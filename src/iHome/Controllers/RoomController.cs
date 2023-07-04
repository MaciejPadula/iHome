using iHome.Core.Models;
using iHome.Core.Services;
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
    private readonly IRoomService _roomService;
    private readonly IUserAccessor _userAccessor;

    public RoomController(IRoomManagementService roomManagementService, IUserAccessor userAccessor, IUserManagementService userManagementService, IRoomSharingService roomSharingService, IRoomService roomService)
    {
        _roomManagementService = roomManagementService;
        _userAccessor = userAccessor;
        _userManagementService = userManagementService;
        _roomSharingService = roomSharingService;
        _roomService = roomService;
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
        return Ok(await _roomService.GetRooms(_userAccessor.UserId));
    }

    [HttpGet("GetRoomUsers/{roomId}")]
    public async Task<IActionResult> GetRoomUsers(Guid roomId)
    {
        return Ok(await _roomService.GetRoomUsers(roomId, _userAccessor.UserId));
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
