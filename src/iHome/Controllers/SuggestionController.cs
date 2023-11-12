using iHome.Core.Services;
using iHome.Models.Requests.Suggestion;
using iHome.Models.Responses.Suggestion;
using iHome.Shared.Controllers;
using iHome.Shared.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace iHome.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SuggestionController : BaseApiController
{
    private readonly ISuggestionService _suggestionService;

    public SuggestionController(ISuggestionService suggestionService, IUserAccessor userAccessor)
        : base(userAccessor)
    {
        _suggestionService = suggestionService;
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
