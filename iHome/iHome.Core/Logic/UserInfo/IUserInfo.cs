using System.Security.Claims;

namespace iHome.Core.Logic.UserInfo
{
    public interface IUserInfo
    {
        string GetUserUuid(ClaimsPrincipal user);
        string GetUserUuid(string email);
        string GetUserEmail(string uuid);
        List<string> GetEmails(string emailTest);
    }
}
