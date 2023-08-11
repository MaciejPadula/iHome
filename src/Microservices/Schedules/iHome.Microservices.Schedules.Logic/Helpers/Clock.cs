namespace iHome.Microservices.Schedules.Logic.Helpers;

public interface IClock
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}

public class Clock : IClock
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;
}
