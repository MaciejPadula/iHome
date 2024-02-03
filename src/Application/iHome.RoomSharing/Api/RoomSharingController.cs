using iHome.RoomSharing.Api.Request;
using iHome.RoomSharing.Features.GetRoomUsers;
using iHome.RoomSharing.Features.ShareRoom;
using iHome.RoomSharing.Features.UnshareRoom;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Cqrs.Mediator;

namespace iHome.RoomSharing.Api;

[Authorize]
public class RoomSharingController : BaseApiController
{
    private readonly IMediator _mediator;

    public RoomSharingController(IMediator mediator, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _mediator = mediator;
    }

    [HttpPost("ShareRoom")]
    public async Task<IActionResult> ShareRoom([FromBody] ShareRoomRequest request)
    {
        await _mediator.HandleCommandAsync(new ShareRoomCommand
        {
            RoomId = request.RoomId,
            UserId = request.UserId,
            CallerId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpPost("UnshareRoom")]
    public async Task<IActionResult> UnshareRoom([FromBody] UnshareRoomRequest request)
    {
        await _mediator.HandleCommandAsync(new UnshareRoomCommand
        {
            RoomId = request.RoomId,
            UserId = request.UserId,
            CallerId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpGet("GetRoomUsers/{roomId}")]
    public async Task<IActionResult> GetRoomUsers(Guid roomId)
    {
        var query = await _mediator.HandleQueryAsync(new GetRoomUsersQuery
        {
            RoomId = roomId,
            UserId = _userAccessor.UserId
        });

        return Ok(query.Result);
    }
}
