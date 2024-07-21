using Authorize.Models;
using BigGrayBison.Authorize.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authorize.Controllers
{
    public class UserController : Controller
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IUserFactory _userFactory;
        private readonly IUserUpdater _userUpdater;

        public UserController(
            ISettingsFactory settingsFactory,
            IUserFactory userFactory,
            IUserUpdater userUpdater)
        {
            _settingsFactory = settingsFactory;
            _userFactory = userFactory;
            _userUpdater = userUpdater;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UsersVM usersVM)
        {
            ISettings settings = _settingsFactory.CreateCore();
            IUser user = await _userFactory.GetByName(settings, usersVM.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Not found");
                return View(usersVM);
            }
            else
            {
                return View(
                "User",
                new UserVM
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    EmailAddress = await user.GetEmailAddress(settings),
                    IsActive = user.IsActive,
                    IsUserEditor = user.IsUserEditor(),
                    CreateTimestamp = user.CreateTimestamp,
                    UpdateTimestamp = user.UpdateTimestamp
                });
            }
        }

        [HttpPost("User/Save")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UserVM userVM)
        {
            ISettings settings = _settingsFactory.CreateCore();
            IUser innerUser = await _userFactory.Get(settings, userVM.UserId);
            if (innerUser == null)
            {
                ModelState.AddModelError(string.Empty, "Not found");
            }
            else
            {
                innerUser.IsActive = userVM.IsActive;
                innerUser.IsUserEditor(userVM.IsUserEditor);
                innerUser.SetEmailAddress(userVM.EmailAddress);
                await _userUpdater.Update(settings, innerUser);
            }
            return View("User", userVM);
        }
    }
}
