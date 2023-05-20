using iHome.Microservices.OpenAI.Contract.Models;
using iHome.Microservices.OpenAI.Infrastructure.Logic;
using System.Text.Json;

namespace iHome.Microservices.OpenAI.Infrastructure.Services;

public class OpenAIScheduleSuggestionsService : IScheduleSuggestionsService
{
    private readonly IOpenAICompletions _openAICompletions;

    public OpenAIScheduleSuggestionsService(IOpenAICompletions openAICompletions)
    {
        _openAICompletions = openAICompletions;
    }

    public async Task<IEnumerable<Guid>> GetDevicesIdsForSchedule(string scheduleName, string scheduleTime, IEnumerable<DeviceDetails> devices)
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

    public async Task<string> GetTimeForSchedule(string scheduleName)
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
