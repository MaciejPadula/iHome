using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using System.Text.Json;

namespace iHome.Microservices.OpenAI.Infrastructure.Logic;

public interface IOpenAICompletions
{
    Task<T> Complete<T>(string prompt, string model = "text-davinci-003", int maxTokens = 250);
    Task<string> CompleteText(string prompt, string model = "text-davinci-003", int maxTokens = 250);
    Task<T> CompleteChatText<T>(string systemPrompt, string userPrompt, string model = "gpt-3.5-turbo", int maxTokens = 250);
    Task<string> CompleteChatText(string systemPrompt, string userPrompt, string model = "gpt-3.5-turbo", int maxTokens = 250);
}

public class OpenAICompletions : IOpenAICompletions
{
    private readonly IOpenAIService _openAIService;

    public OpenAICompletions(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    public async Task<T> Complete<T>(string prompt, string model, int maxTokens)
    {
        var completedText = await CompleteText(prompt, model, maxTokens);

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

    public async Task<string> CompleteText(string prompt, string model, int maxTokens)
    {
        var competition = await _openAIService.Completions.CreateCompletion(new CompletionCreateRequest
        {
            Prompt = prompt.Trim(),
            Model = model,
            MaxTokens = maxTokens
        });

        if (!(competition?.Successful ?? false)) return string.Empty;

        return competition?.Choices?.FirstOrDefault()?.Text?.Trim() ?? string.Empty;
    }

    public async Task<T> CompleteChatText<T>(string systemPrompt, string userPrompt, string model = "gpt-3.5-turbo", int maxTokens = 250)
    {
        var completedText = await CompleteChatText(systemPrompt, userPrompt, model, maxTokens);

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

    public async Task<string> CompleteChatText(string systemPrompt, string userPrompt, string model, int maxTokens)
    {
        var chatMessages = new List<ChatMessage>
        {
            ChatMessage.FromSystem(systemPrompt),
            ChatMessage.FromUser(userPrompt)
        };

        var competition = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = chatMessages,
            Model = model,
            MaxTokens = maxTokens
        });

        if (!(competition?.Successful ?? false)) return string.Empty;

        return competition?.Choices?.FirstOrDefault()?.Message?.Content ?? string.Empty;
    }
}