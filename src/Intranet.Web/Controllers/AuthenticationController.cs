using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Intranet.Web.Authentication.Providers;
using Intranet.Web.Authentication.Services;
using Microsoft.AspNetCore.Authentication;

namespace Intranet.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly ICustomAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationProvider authenticationProvider,
                                        ICustomAuthenticationService authenticationService)
        {
            _authenticationProvider = authenticationProvider;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _authenticationService.VerifyUser(username, password);
                var claimsPrinciple = _authenticationProvider.GetClaimsPrincipal(user);

                if (claimsPrinciple == null)
                {
                    ViewData["ReturnUrl"] = returnUrl;
                    ModelState.AddModelError("Login", "Username or password is incorrect.");
                    return View();
                };

                await HttpContext.SignInAsync(claimsPrinciple);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/");
        }
    }
}
