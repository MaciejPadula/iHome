namespace iHome.Logic;

public class MockUserAccessor : IUserAccessor
{
    public Guid UserId => Guid.Parse("237fc249-9902-45d7-b7fb-40e1e074b7ad");

    public string Name => "Maciej";

    public string Email => "maciejtest@example.com";
}
