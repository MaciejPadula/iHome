using iHome.Core.Services;
using iHome.Infrastructure.OpenAI.Models;
using iHome.Infrastructure.OpenAI.Services;
using iHome.Infrastructure.OpenAI.Services.Suggestions;
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
    private readonly IDevicesForSchedulingAccessor _devicesForSchedulingAccessor;
    private readonly ISuggestionsService _suggestionsService;

    private readonly IUserAccessor _userAccessor;

    public SuggestionController(IDevicesForSchedulingAccessor devicesForSchedulingAccessor, ISuggestionsService suggestionsService, IUserAccessor userAccessor)
    {
        _devicesForSchedulingAccessor = devicesForSchedulingAccessor;
        _suggestionsService = suggestionsService;
        _userAccessor = userAccessor;
    }

    [HttpPost("GetSuggestedHour")]
    public async Task<IActionResult> GetSuggestedHour(GetSuggestedHourRequest request)
    {
        var response = await _suggestionsService.GetSuggestedTimeByScheduleName(request.ScheduleName);
        var txt = response.Split(":");
        if (txt.Length != 2) return Ok(null);

        return Ok(new GetSuggestedHourResponse
        {
            Hour = int.Parse(txt[0]),
            Minute = int.Parse(txt[1])
        });
    }

    [HttpPost("GetSuggestedDevices")]
    public async Task<IActionResult> GetSuggestedDevices(GetSuggestedDevicesRequest request)
    {
        var devices = (await _devicesForSchedulingAccessor
            .Get(_userAccessor.UserId))
            .Select(d => new OpenAIRequestDevice
            {
                Id = d.Id,
                Name = d.Name,
                Type = d.Type.ToString()
            });

        var suggested = await _suggestionsService.GetDevicesThatCouldMatchSchedule(request.ScheduleName, request.ScheduleTime, devices);

        return Ok(suggested);
    }
}
