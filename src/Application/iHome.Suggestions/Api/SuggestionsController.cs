using iHome.DevicesScheduling.Features.GetDevicesForScheduling;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using iHome.Suggestions.Api.Request;
using iHome.Suggestions.Api.Response;
using iHome.Suggestions.Features.GetSuggestedDevices;
using iHome.Suggestions.Features.GetSuggestedTime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure.Cqrs.Mediator;

namespace iHome.Suggestions.Api;

[Authorize]
public class SuggestionsController : BaseApiController
{
    private readonly IMediator _mediator;

    public SuggestionsController(IUserAccessor userAccessor, IMediator mediator)
        : base(userAccessor)
    {
        _mediator = mediator;
    }

    [HttpPost("GetSuggestedHour")]
    public async Task<IActionResult> GetSuggestedHour(GetSuggestedHourRequest request)
    {
        var query = await _mediator.HandleQueryAsync(new GetSuggestedTimeQuery
        {
            Name = request.ScheduleName
        });

        var result = query.Result;

        return Ok(new GetSuggestedHourResponse
        {
            Hour = result.Hour,
            Minute = result.Minute
        });
    }

    [HttpPost("GetSuggestedDevices")]
    public async Task<IActionResult> GetSuggestedDevices(GetSuggestedDevicesRequest request)
    {
        var devicesQuery = await _mediator.HandleQueryAsync(new GetDevicesForSchedulingQuery
        {
            UserId = _userAccessor.UserId
        });

        var query = await _mediator.HandleQueryAsync(new GetSuggestedDevicesQuery
        {
            Name = request.ScheduleName,
            Time = request.ScheduleTime,
            Devices = devicesQuery.Result
        });

        return Ok(query.Result);
    }
}
