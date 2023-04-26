namespace iHome.Scheduler.Contexts;

public class WorkerContext
{
    public bool IsRunning { get; set; } = true;
    public TimeSpan JobDelay { get; set; } = TimeSpan.FromSeconds(1);
}
