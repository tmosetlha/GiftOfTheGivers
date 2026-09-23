using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Controllers
{
    [Authorize] // must be logged in — runs right after Register
    public class AccountSetupController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountSetupController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /AccountSetup/ChooseRole
        public async Task<IActionResult> ChooseRole()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count > 0)
            {
                // Already has a role — no need to choose again.
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: /AccountSetup/ChooseRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseRole(string role)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            var chosenRole = (role == "Employee") ? "Employee" : "Donor";
            await _userManager.AddToRoleAsync(user, chosenRole);

            return RedirectToAction("Index", "Home");
        }
    }
}