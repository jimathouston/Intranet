using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intranet.Web.Providers.Contracts;
using Intranet.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intranet.Web.Controllers
{
  public class AuthenticationController : Controller
  {
    private readonly IAuthenticationProvider _authenticationProvider;
    private readonly ITokenProvider _tokenProvider;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationProvider authenticationProvider,
                                    IAuthenticationService authenticationService,
                                    ITokenProvider tokenProvider)
    {
      _authenticationProvider = authenticationProvider;
      _authenticationService = authenticationService;
      _tokenProvider = tokenProvider;
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
      return new JsonResult(new { success = true });
    }

    [HttpGet]
    public IActionResult GenerateToken()
    {
      var user = HttpContext.User;
      var response = _tokenProvider.GenerateToken(user);

      return new JsonResult(new { accessToken = response.accessToken, expiresIn = response.expiresIn });
    }
  }
}