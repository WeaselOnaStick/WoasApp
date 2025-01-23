using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using WoasApp.Areas.Identity.Data;
using WoasApp.ViewModels;

namespace WoasApp.Controllers
{
    public class DashboardController : Controller
    {
        private UserManager<WoasAppUser> UserManager;
        private SignInManager<WoasAppUser> SignInManager;

        public DashboardController(UserManager<WoasAppUser> userManager, SignInManager<WoasAppUser> signInManager)
        {
            UserManager = userManager;
        }

        async public Task<IActionResult> Index()
        {
            var users = await UserManager.Users.Include(u => u.LoginTimes).ToListAsync();
            var usersViewModel = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                Blocked = u.Blocked,
                LastLogin = u.LoginTimes.IsNullOrEmpty() ? null : u.LoginTimes.OrderByDescending(lt => lt.LoginTime).First().LoginTime,
                IsSelected = false,
            }).OrderByDescending(u => u.LastLogin).ToList();


            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsersAsync(List<string> SelectedUserIds)
        {
            bool delSelf = false;
            string curUserID = UserManager.GetUserId(User);
            foreach (var userId in SelectedUserIds)
            {
                delSelf = userId == curUserID;
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                    await UserManager.DeleteAsync(user);
            }
            return RedirectToAction("Index", delSelf ? "Home" : "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> BlockUsersAsync(List<string> SelectedUserIds)
        {
            return await SetBlockUsersAsync(SelectedUserIds, true);
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUsersAsync(List<string> SelectedUserIds)
        {
            return await SetBlockUsersAsync(SelectedUserIds, false);
        }


        private async Task<IActionResult> SetBlockUsersAsync(List<string> SelectedUserIds, bool block)
        {
            foreach (var userId in SelectedUserIds)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.Blocked = block;
                    await UserManager.UpdateAsync(user);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
