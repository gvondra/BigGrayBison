using Authorize.Models;
using BigGrayBison.Authorize.Framework;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authorize.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<Settings> _settings;
        private readonly ILogger<HomeController> _logger;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public HomeController(
            IOptions<Settings> settings,
            ILogger<HomeController> logger,
            ISettingsFactory settingsFactory,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _settings = settings;
            _logger = logger;
            _settingsFactory = settingsFactory;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task<IActionResult> Index([FromQuery] string code)
        {
            IActionResult result = null;
            InitializeViewBag();
            if (!string.IsNullOrEmpty(code))
                result = await Login(code);
            return result ?? View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [NonAction]
        private void InitializeViewBag()
        {
            ViewData["LoginClientId"] = _settings.Value.LoginClientId.Value.ToString("D");
            ViewData["LoginRedirectUrl"] = _settings.Value.LoginRedirectUrl;
        }

        [NonAction]
        private async Task<IActionResult> Login(string code)
        {
            JwtSecurityToken jwtSecurityToken = await _accessTokenGenerator.GenerateForAuthorizationCode(_settingsFactory.CreateCore(), _settings.Value.LoginClientId.Value, code);
            if (jwtSecurityToken != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(jwtSecurityToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                AuthenticationProperties authenticationProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                    IsPersistent = false,
                    IssuedUtc = DateTime.UtcNow
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
