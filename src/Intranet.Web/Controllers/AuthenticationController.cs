using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Intranet.Web.Authentication.Providers;
using Intranet.Web.Authentication.Services;

namespace Intranet.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationProvider authenticationProvider,
                                        IAuthenticationService authenticationService)
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
            var user = _authenticationService.VerifyUser(username, password);
            var claimsPrinciple = _authenticationProvider.GetClaimsPrincipal(user);

            if (claimsPrinciple == null) return View();

            await HttpContext.Authentication.SignInAsync("Cookies", claimsPrinciple);

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return Redirect("~/");
        }
    }
}
