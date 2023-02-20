using System.Collections.Generic;
using System.Security.Claims;

namespace iHome.HubApp.Services.UserService;

public interface IUserService
{
    string Name { get; }
    string Email { get; }
    bool IsAuthenticated { get; }

    void Login();
    void Logout();
    
}
