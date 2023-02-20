using System.Security.Claims;

namespace iHome.HubApp.Logic.ClaimsResolver;

public class ClaimsResolver : IClaimsResolver
{
    public string GetClaimValue(ClaimsPrincipal? claimsPrincipal, string claimType)
    {
        return claimsPrincipal?.FindFirst(c => c.Type == claimType)?.Value ?? string.Empty;
    }
}
