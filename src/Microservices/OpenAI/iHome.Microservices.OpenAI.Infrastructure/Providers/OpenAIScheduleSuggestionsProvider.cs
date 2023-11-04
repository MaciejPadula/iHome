using iHome.Microservices.OpenAI.Contract.Models;
using iHome.Microservices.OpenAI.Infrastructure.Logic;
using iHome.Microservices.OpenAI.Model;
using OpenAI.ObjectModels;
using System.Text.Json;

namespace iHome.Microservices.OpenAI.Infrastructure.Providers;

public class OpenAIScheduleSuggestionsProvider : IScheduleSuggestionsProvider
{
    private readonly IOpenAICompletions _openAICompletions;

    public OpenAIScheduleSuggestionsProvider(IOpenAICompletions openAICompletions)
    {
        _openAICompletions = openAICompletions;
    }

    public async Task<IEnumerable<Guid>> GetDevicesIdsForSchedule(string scheduleName, string scheduleTime, IEnumerable<DeviceDetails> devices)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        var system =
"""
Result should be presented as JSON array of string!!! Do not return any meaningless text!!!

User will provide you list of devices with their ids, names and types.
Addiditonally you will receive name and run hour of some scheduled task.
With this information filter given list of devices and return only matching devices.
Select only ids of devices, remove name and type
""";
        var prompt =
$"""
ScheduleName: {scheduleName}
ScheduleTime: {scheduleTime}
ListOfDevices:
{JsonSerializer.Serialize(devices, options)}
""";

        return await _openAICompletions.CompleteChatText<IEnumerable<Guid>>(
            system,
            prompt,
            Models.Gpt_4) ?? Enumerable.Empty<Guid>();
    }

    public async Task<string> GetTimeForSchedule(string scheduleName)
    {
        var system =
"""
Provide time in format HH:MM. Any other format is unacceptable!
Important: do not return anything but time in format HH:MM!!!

You are looking for the best suiting hour for some scheduled named task to run.
TaskName will be provided from user:
[TaskName]

Return hour that would suit users need in HH:MM format!
""";

        var user =
$"""
TaskName: {scheduleName}
""";

        return await _openAICompletions.CompleteChatText(system, user, Models.Gpt_4);
    }
}
