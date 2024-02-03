using iHome.RoomsList.Api.Request;
using iHome.RoomsList.Api.Response;
using iHome.RoomsList.Features.AddRoom;
using iHome.RoomsList.Features.GetUserRooms;
using iHome.RoomsList.Features.RemoveRoom;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Cqrs.Mediator;

namespace iHome.RoomsList.Api;

[Authorize]
public class RoomsListController : BaseApiController
{
    private readonly IMediator _mediator;

    public RoomsListController(IUserAccessor userAccessor, IMediator mediator)
        : base(userAccessor)
    {
        _mediator = mediator;
    }

    [HttpPost("AddRoom")]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomRequest request)
    {
        await _mediator.HandleCommandAsync(new AddRoomCommand
        {
            RoomName = request.RoomName,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpGet("GetRooms")]
    public async Task<IActionResult> GetRooms()
    {
        var query = await _mediator.HandleQueryAsync(new GetUserRoomsQuery
        {
            UserId = _userAccessor.UserId
        });

        return Ok(new GetRoomsResponse { Rooms = query.Result });
    }

    [HttpDelete("RemoveRoom/{roomId}")]
    public async Task<IActionResult> RemoveRoom(Guid roomId)
    {
        await _mediator.HandleCommandAsync(new RemoveRoomCommand
        {
            RoomId = roomId,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }
}
