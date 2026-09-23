using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Employee  — dashboard: volunteer sign-ups
        public async Task<IActionResult> Index()
        {
            var volunteers = await _context.Volunteers
                .OrderByDescending(v => v.RegisteredDate)
                .ToListAsync();
            return View(volunteers);
        }

        // GET: /Employee/PostUpdate
        public async Task<IActionResult> PostUpdate()
        {
            ViewBag.Projects = await _context.ReliefProjects.ToListAsync();
            return View(new ReliefUpdate());
        }

        // POST: /Employee/PostUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostUpdate(ReliefUpdate update)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Projects = await _context.ReliefProjects.ToListAsync();
                return View(update);
            }

            update.EmployeeId = _userManager.GetUserId(User)!;
            update.PostedDate = DateTime.UtcNow;

            _context.ReliefUpdates.Add(update);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Update posted successfully.";
            return RedirectToAction(nameof(PostUpdate));
        }
    }
}