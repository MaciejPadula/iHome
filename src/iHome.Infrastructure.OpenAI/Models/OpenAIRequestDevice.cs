namespace iHome.Infrastructure.OpenAI.Models;

public class OpenAIRequestDevice
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Type { get; set; } = default!;
}
