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
            SignInManager = signInManager;
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
            return await ModifyUsersAsync(SelectedUserIds, UserManageAction.Delete);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUsersAsync(List<string> SelectedUserIds)
        {
            return await ModifyUsersAsync(SelectedUserIds, UserManageAction.Block);
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUsersAsync(List<string> SelectedUserIds)
        {
            return await ModifyUsersAsync(SelectedUserIds, UserManageAction.Unblock);
        }

        enum UserManageAction
        {
            Block,
            Unblock,
            Delete
        }

        private Dictionary<UserManageAction, string> UserManageActionMessages = new Dictionary<UserManageAction, string>
        {
            { UserManageAction.Block, "Blocked" },
            { UserManageAction.Unblock, "Unblocked" },
            { UserManageAction.Delete, "Deleted" }
        };

        private async Task<IActionResult> ModifyUsersAsync(List<string> SelectedUserIds, UserManageAction action)
        {
            string currentUserId = UserManager.GetUserId(User);
            bool foundCurrentUser = false;

            foreach (var userId in SelectedUserIds)
            {
                foundCurrentUser = (currentUserId == userId) || foundCurrentUser;
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    switch (action)
                    {
                        case UserManageAction.Block:
                            user.Blocked = true;
                            break;
                        case UserManageAction.Unblock:
                            user.Blocked = false;
                            break;
                        case UserManageAction.Delete:
                            await UserManager.DeleteAsync(user);
                            break;
                    }
                    await UserManager.UpdateAsync(user);
                }
            }
            bool isCurrentDeletedOrBlocked = foundCurrentUser && (action == UserManageAction.Block || action == UserManageAction.Delete);
            
            if (isCurrentDeletedOrBlocked){
                await SignInManager.SignOutAsync();
                return RedirectToAction("Logout", "Account");
            }

            TempData["ModifyUsersAsyncResult"] = $"{UserManageActionMessages[action]} {SelectedUserIds.Count} user{(SelectedUserIds.Count == 1 ? "" : "s")}!";
            return RedirectToAction("Index");
        }
    }
}
