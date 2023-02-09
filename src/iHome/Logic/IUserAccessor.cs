namespace iHome.Logic;

public interface IUserAccessor
{
    string UserId { get; }
    string Name { get; }
    string Email { get; }
}
