using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoriesOfTheLand.Data;
using StoriesOfTheLand.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace StoriesOfTheLand.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Initiates the sign-in process by redirecting the user to the Microsoft login page.
        /// </summary>
        /// <returns>The IActionResult that redirects the user to the login page.</returns>
        [HttpGet]
        public IActionResult SignIn()
        {
            var redirectUrl = Url.Action(nameof(HomeController.Index), "Home");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Signs out the user and disposes of authentication cookies.
        /// </summary>
        /// <returns>The IActionResult for signing out.</returns>
        [HttpGet]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
        /// <summary>
        /// Redirects to the home page after user signs out.
        /// </summary>
        /// <returns>The IActionResult for the signed-out state.</returns>
        [HttpGet]
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return RedirectToAction(nameof(HomeController.Index), "/");
        }
        /// <summary>
        /// Handles access denied scenarios.
        /// </summary>
        /// <returns>The IActionResult for the access denied page.</returns>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}