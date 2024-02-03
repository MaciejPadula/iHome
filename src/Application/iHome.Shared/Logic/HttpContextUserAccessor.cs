using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace iHome.Shared.Logic;

public class HttpContextUserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => GetClaimValueByType(ClaimTypes.NameIdentifier);
    public string Name => GetClaimValueByType(ClaimTypes.Name);
    public string Email => GetClaimValueByType(ClaimTypes.Email);

    public Claim? FindClaimByType(string type) => _httpContextAccessor.HttpContext?.User?.FindFirst(c => c.Type == type);

    public string GetClaimValueByType(string type) => FindClaimByType(type)?.Value ?? string.Empty;
}
