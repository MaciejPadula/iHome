using iHome.Core.Services;
using iHome.Logic;
using iHome.Models.Requests.Suggestion;
using iHome.Models.Responses.Suggestion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SuggestionController : ControllerBase
{
    private readonly ISuggestionService _suggestionService;
    private readonly IUserAccessor _userAccessor;

    public SuggestionController(ISuggestionService suggestionService, IUserAccessor userAccessor)
    {
        _suggestionService = suggestionService;
        _userAccessor = userAccessor;
    }

    [HttpPost("GetSuggestedHour")]
    public async Task<IActionResult> GetSuggestedHour(GetSuggestedHourRequest request)
    {
        var time = await _suggestionService.GetSuggestedTime(request.ScheduleName);

        return Ok(new GetSuggestedHourResponse
        {
            Hour = time.Hour,
            Minute = time.Minute
        });
    }

    [HttpPost("GetSuggestedDevices")]
    public async Task<IActionResult> GetSuggestedDevices(GetSuggestedDevicesRequest request)
    {
        var devices = await _suggestionService.GetSuggestedDevices(
            request.ScheduleName, request.ScheduleTime, _userAccessor.UserId);

        return Ok(devices);
    }
}
