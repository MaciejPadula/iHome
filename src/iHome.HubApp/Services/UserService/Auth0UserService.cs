using Auth0.OidcClient;
using iHome.HubApp.Exceptions;
using iHome.HubApp.Logic.ClaimsResolver;
using System;
using System.Security.Claims;

namespace iHome.HubApp.Services.UserService;

public class Auth0UserService : IUserService
{
    private readonly IClaimsResolver _claimsResolver;
    private readonly Auth0Client _authClient;
    private ClaimsPrincipal? _user;

    public Auth0UserService(IClaimsResolver claimsResolver, Auth0Client authClient)
    {
        _claimsResolver = claimsResolver;
        _authClient = authClient;
    }

    public async void Login()
    {
        var loginResult = await _authClient.LoginAsync();

        if (loginResult.IsError) throw new UserNotAuthenticatedException(loginResult.Error);

        _user = loginResult.User;
    }

    public string Name => _claimsResolver.GetClaimValue(_user, "name");

    public string Email => _claimsResolver.GetClaimValue(_user, "email");

    public bool IsAuthenticated => _user != null;

    public void Logout()
    {
        _authClient.LogoutAsync();
        _user = null;
    }
}
