using Authorize.Models;
using BigGrayBison.Authorize.Framework;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authorize.Controllers
{
    public class CreateUserController : Controller
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserValidator _userValidator;
        private readonly ISettingsFactory _settingsFactory;

        public CreateUserController(
            IUserFactory userFactory,
            IUserValidator userValidator,
            ISettingsFactory settingsFactory)
        {
            _userFactory = userFactory;
            _userValidator = userValidator;
            _settingsFactory = settingsFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreateUserVM createUserVM)
        {
            if (ModelState.IsValid)
            {
                await ValidateUserName(createUserVM.UserName);
                ValidatePassword(createUserVM.Password1, createUserVM.Password2);
            }
            return View(createUserVM);
        }

        [NonAction]
        private async Task ValidateUserName(string userName)
        {
            string message = _userValidator.ValidateUserName(userName);
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError(string.Empty, message);
            }
            else if (!await _userFactory.GetUserNameAvailable(_settingsFactory.CreateCore(), userName))
            {
                ModelState.AddModelError(string.Empty, "User name unavailable");
            }
        }

        [NonAction]
        private void ValidatePassword(string password1, string password2)
        {
            if (!string.Equals(password1 ?? string.Empty, password2 ?? string.Empty))
            {
                ModelState.AddModelError(string.Empty, "Entered passwords differ from each other.");
            }
            else
            {
                string message = _userValidator?.ValidatePassword(password1);
                if (!string.IsNullOrEmpty(message))
                    ModelState.AddModelError(string.Empty, message);
            }
        }
    }
}
