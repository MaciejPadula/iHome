using iHome.Model;

namespace iHome.Users.Api.Response;

public class GetUsersResponse
{
    public required List<UserDto> Users { get; set; }
}
