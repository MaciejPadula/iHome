using iHome.SchedulesList.Api.Request;
using iHome.SchedulesList.Api.Response;
using iHome.SchedulesList.Feature.AddSchedule;
using iHome.SchedulesList.Feature.GetUserSchedulesOrdered;
using iHome.SchedulesList.Feature.RemoveSchedule;
using iHome.SchedulesList.Feature.UpdateScheduleTime;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Cqrs.Mediator;
using AddScheduleRequest = iHome.SchedulesList.Api.Request.AddScheduleRequest;

namespace iHome.SchedulesList.Api;

[Authorize]
public class SchedulesListController : BaseApiController
{
    private readonly IMediator _mediator;

    public SchedulesListController(IMediator mediator, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddSchedule([FromBody] AddScheduleRequest request)
    {
        await _mediator.HandleCommandAsync(new AddScheduleCommand
        {
            Name = request.ScheduleName,
            Day = request.Day,
            Time = request.ScheduleTime,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpGet("GetSchedules")]
    public async Task<IActionResult> GetSchedules()
    {
        var query = await _mediator.HandleQueryAsync(new GetUserSchedulesOrderedQuery
        {
            UserId = _userAccessor.UserId
        });

        return Ok(new GetSchedulesResponse
        {
            Schedules = query.Result
        });
    }

    [HttpPost("UpdateSchedule")]
    public async Task<IActionResult> UpdateSchedule([FromBody] UpdateScheduleRequest request)
    {
        await _mediator.HandleCommandAsync(new UpdateScheduleTimeCommand
        {
            Id = request.ScheduleId,
            Time = request.ScheduleTime,
            Day = request.Day,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }

    [HttpDelete("RemoveSchedule/{id}")]
    public async Task<IActionResult> RemoveSchedule(Guid id)
    {
        await _mediator.HandleCommandAsync(new RemoveScheduleCommand
        {
            Id = id,
            UserId = _userAccessor.UserId
        });
        return Ok();
    }
}
