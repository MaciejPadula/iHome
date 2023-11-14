namespace iHome.Model;

public class RoomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public UserDto? User { get; set; }
}
