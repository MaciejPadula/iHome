using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace iHome.HubApp.Services.UserService;

public interface IUserService
{
    string Name { get; }
    string Email { get; }
    bool IsAuthenticated { get; }
    string AccessToken { get; }

    Task Login();
    void Logout();
    
}
