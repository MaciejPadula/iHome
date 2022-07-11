
using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using iHome.Models.Account;

namespace iHome.Controllers
{
    public class AccountController : Controller
    {
        public async Task Login(string returnUrl = "/Account/Rooms")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }
        [Authorize]
        public IActionResult Profile()
        {
            
            return View(new ProfileModel()
            {
                Name = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value,
                EmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.FindFirst(c => c.Type == "picture")?.Value,
                Uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            });
        }
        [Authorize]
        public IActionResult Rooms()
        {
            return View(new ProfileModel()
            {
                Name = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value,
                EmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.FindFirst(c => c.Type == "picture")?.Value,
                Uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            });
        }

        public IActionResult RoomsOld()
        {
            return View(new ProfileModel()
            {
                Name = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value,
                EmailAddress = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.FindFirst(c => c.Type == "picture")?.Value,
                Uuid = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            });
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
