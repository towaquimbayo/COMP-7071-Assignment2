using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace _7071Sprint1Demo.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = "/Employees")
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = returnUrl
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return SignOut(
                new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
