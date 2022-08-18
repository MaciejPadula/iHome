using System.Security.Claims;

namespace iHome.Logic.UserInfo
{
    public interface IUserInfo
    {
        Task<string?> GetPublicIp(HttpContext httpContext);
        string? GetUserUuid(ClaimsPrincipal user);
        string? GetUserUuid(string email);
        string? GetUserEmail(string uuid);
    }
}
