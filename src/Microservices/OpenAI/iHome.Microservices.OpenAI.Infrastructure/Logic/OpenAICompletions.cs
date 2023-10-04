using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using System.Text.Json;

namespace iHome.Microservices.OpenAI.Infrastructure.Logic;

public interface IOpenAICompletions
{
    Task<T> Complete<T>(string prompt, string mode = "text-davinci-003", int maxTokens = 250);
    Task<string> CompleteText(string prompt, string mode = "text-davinci-003", int maxTokens = 250);
}

public class OpenAICompletions : IOpenAICompletions
{
    private readonly IOpenAIService _openAIService;

    public OpenAICompletions(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    public async Task<T> Complete<T>(string prompt, string mode, int maxTokens)
    {
        var completedText = await CompleteText(prompt, mode, maxTokens);

        try
        {
            var parsedList = JsonSerializer.Deserialize<T>(completedText);
            return parsedList ?? default!;
        }
        catch
        {
            return default!;
        }
    }

    public async Task<string> CompleteText(string prompt, string mode, int maxTokens)
    {
        var competition = await _openAIService.Completions.CreateCompletion(new CompletionCreateRequest
        {
            Prompt = prompt.Trim(),
            Model = mode,
            MaxTokens = maxTokens
        });

        if (!(competition?.Successful ?? false)) return string.Empty;

        return competition?.Choices?.FirstOrDefault()?.Text?.Trim() ?? string.Empty;
    }
}