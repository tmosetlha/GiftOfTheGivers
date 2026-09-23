using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VolunteerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Volunteer
        public IActionResult Index()
        {
            return View(new Volunteer());
        }

        // POST: /Volunteer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Volunteer volunteer)
        {
            if (!ModelState.IsValid)
            {
                return View(volunteer);
            }

            // Link to account if logged in; otherwise it's a lightweight sign-up (UserId stays null).
            if (User.Identity is { IsAuthenticated: true })
            {
                volunteer.UserId = _userManager.GetUserId(User);
            }

            volunteer.RegisteredDate = DateTime.UtcNow;
            volunteer.IsConfirmed = false; // an employee confirms sign-ups later

            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ThankYou));
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
