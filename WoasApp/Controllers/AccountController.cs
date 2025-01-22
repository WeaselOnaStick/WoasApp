using Microsoft.AspNetCore.Identity;
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

            var res = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.Remember, false);

            if (!res.Succeeded)
            {
                ModelState.AddModelError("InvalidLogin", "Login or password is incorrect!");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            WoasAppUser user = new WoasAppUser
            {
                UserName = model.Name,
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
    }
}
