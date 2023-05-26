namespace iHome.Jobs.Events.Infrastructure.Helpers.DateTimeProvider;

public class DefaultDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;

    public DateTime UtcNow => DateTime.UtcNow;
}
