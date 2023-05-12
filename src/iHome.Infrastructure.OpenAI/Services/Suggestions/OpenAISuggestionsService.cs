using iHome.Infrastructure.OpenAI.Logic;
using iHome.Infrastructure.OpenAI.Models;
using System.Text.Json;

namespace iHome.Infrastructure.OpenAI.Services.Suggestions;

public class OpenAISuggestionsService : ISuggestionsService
{
    private readonly IOpenAICompletions _openAICompletions;

    public OpenAISuggestionsService(IOpenAICompletions openAICompletions)
    {
        _openAICompletions = openAICompletions;
    }

    public async Task<IEnumerable<Guid>> GetDevicesThatCouldMatchSchedule(string scheduleName, string scheduleTime, IEnumerable<OpenAIRequestDevice> devices)
    {
        var prompt =
$"""
Given a list of devices with ids, names, and types:
{JsonSerializer.Serialize(devices)}
and schedule routine named {scheduleName} that will be runned at {scheduleTime} please give me devices that you think will work fine with given context.
Return ids of remaining as json list of strings.
""";

        return await _openAICompletions.Complete<IEnumerable<Guid>>(prompt) ?? Enumerable.Empty<Guid>();
    }

    public async Task<string> GetSuggestedTimeByScheduleName(string scheduleName)
    {
        var prompt =
$"""
Provide me time that would fit schedule named {scheduleName}.
This thime should be in HH:MM format and avoid any text.
When you dont know what to answer give me random time in the same format.
""";

        return await _openAICompletions.CompleteText(prompt);
    }
}
