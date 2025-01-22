using Microsoft.AspNetCore.Mvc;

namespace WoasApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
