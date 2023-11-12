namespace iHome.Model;

public class ScheduleDto
{
    public Guid Id { get; set; } = default;
    public string Name { get; set; } = default!;
    public int Hour { get; set; } = default;
    public int Minute { get; set; } = default;
    public bool Runned { get; set; } = default;
    public string UserId { get; set; } = default!;
}
