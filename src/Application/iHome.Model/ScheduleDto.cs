namespace iHome.Model;

public class ScheduleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public int Day { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
    public bool Runned { get; set; }
    public string UserId { get; set; } = default!;
}
