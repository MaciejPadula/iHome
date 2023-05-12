using iHome.Infrastructure.OpenAI.Models;

namespace iHome.Infrastructure.OpenAI.Services.Suggestions;

public interface ISuggestionsService
{
    Task<IEnumerable<Guid>> GetDevicesThatCouldMatchSchedule(string scheduleName, string scheduleTime, IEnumerable<OpenAIRequestDevice> devices);
    Task<string> GetSuggestedTimeByScheduleName(string scheduleName);
}
