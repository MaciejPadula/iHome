namespace iHome.Infrastructure.Queue.Models;

public class QueueOptions<T>
{
    public string ConnectionString { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
}
