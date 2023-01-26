namespace iHome.Logic;

public interface IUserAccessor
{
    Guid UserId { get; }
    string Name { get; }
    string Email { get; }
}
