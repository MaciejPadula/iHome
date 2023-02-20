using System.Security.Claims;

namespace iHome.HubApp.Logic.ClaimsResolver;

public interface IClaimsResolver
{
    string GetClaimValue(ClaimsPrincipal? claimsPrincipal, string claimType);
}
