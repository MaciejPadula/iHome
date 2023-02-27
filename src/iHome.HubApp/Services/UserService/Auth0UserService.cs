using Auth0.OidcClient;
using IdentityModel.OidcClient;
using iHome.HubApp.Exceptions;
using iHome.HubApp.Logic.ClaimsResolver;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace iHome.HubApp.Services.UserService;

public class Auth0UserService : IUserService
{
    private readonly IClaimsResolver _claimsResolver;
    private readonly IAuth0Client _authClient;
    private ClaimsPrincipal? _user;
    private string? _accessToken;

    public Auth0UserService(IClaimsResolver claimsResolver, IAuth0Client authClient)
    {
        _claimsResolver = claimsResolver;
        _authClient = authClient;

        
    }

    public async Task Login()
    {
        var loginResult = await _authClient.LoginAsync();

        if (loginResult.IsError) throw new UserNotAuthenticatedException(loginResult.Error);
        
        _user = loginResult.User;
        _accessToken = loginResult.AccessToken;
    }

    public string Name => _claimsResolver.GetClaimValue(_user, "name");

    public string Email => _claimsResolver.GetClaimValue(_user, "email");

    public bool IsAuthenticated => _user != null;

    public string AccessToken => _accessToken ?? string.Empty;

    public void Logout()
    {
        _authClient.LogoutAsync();
        _user = null;
    }
}
