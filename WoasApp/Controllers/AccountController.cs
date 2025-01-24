using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using WoasApp.Areas.Identity.Data;
using WoasApp.ViewModels;

namespace WoasApp.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<WoasAppUser> signInManager;
        private UserManager<WoasAppUser> userManager;

        public AccountController(SignInManager<WoasAppUser> signInManager, UserManager<WoasAppUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByNameAsync(model.Username);

            if (user != null && user.Blocked)
            {
                ModelState.AddModelError("Blocked", "Your account is blocked!");
                return View(model);
            }

            var res = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.Remember, false);

            if (!res.Succeeded)
            {
                ModelState.AddModelError("InvalidLogin", "Login or password is incorrect!");
                return View(model);
            }


            if (user.LoginTimes == null)
                user.LoginTimes = new List<UserLoginTime>();
            user.LoginTimes.Add(new UserLoginTime { LoginTime = DateTime.UtcNow });
            await userManager.UpdateAsync(user);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            WoasAppUser user = new WoasAppUser
            {
                UserName = model.Username,
                Email = model.Email,
                Blocked = false
            };

            var res = await userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded)
            {
                res.Errors.ToList().ForEach(e => ModelState.AddModelError(e.Code, e.Description));
                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await signInManager.SignOutAsync();
            TempData["Message"] = "You have been logged out!";
            return RedirectToAction("Index", "Home");
        }
    }
}
