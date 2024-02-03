using System.Security.Claims;

namespace iHome.Shared.Logic;

public interface IUserAccessor
{
    string UserId { get; }
    string Name { get; }
    string Email { get; }

    Claim? FindClaimByType(string type);
    string GetClaimValueByType(string type);
}
