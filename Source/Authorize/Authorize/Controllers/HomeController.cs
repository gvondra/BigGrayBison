using Authorize.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Authorize.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<Settings> _settings;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IOptions<Settings> settings, ILogger<HomeController> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public IActionResult Index()
        {
            InitializeViewBag();
            return View();
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

        private void InitializeViewBag()
        {
            ViewData["LoginClientId"] = _settings.Value.LoginClientId;
            ViewData["LoginRedirectUrl"] = _settings.Value.LoginRedirectUrl;
        }
    }
}
