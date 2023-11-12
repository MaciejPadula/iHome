namespace iHome.Model;

public class RoomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserId { get; set; }
    public UserDto? User { get; set; }
}
