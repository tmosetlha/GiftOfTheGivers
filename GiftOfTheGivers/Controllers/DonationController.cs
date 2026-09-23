using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DonationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Donation
        public async Task<IActionResult> Index()
        {
            ViewBag.Projects = await _context.ReliefProjects.ToListAsync();
            return View(new Donation());
        }

        // POST: /Donation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Projects = await _context.ReliefProjects.ToListAsync();
                return View(donation);
            }

            // Logged-in donors are linked to their account unless they tick "anonymous".
            // Guests (not logged in) are always anonymous.
            if (User.Identity is { IsAuthenticated: true } && !donation.IsAnonymous)
            {
                donation.DonorId = _userManager.GetUserId(User);
            }
            else
            {
                donation.DonorId = null;
                donation.IsAnonymous = true;
            }

            donation.DonationDate = DateTime.UtcNow;
            donation.Status = "Completed"; // prototype stage — no real payment gateway yet
            donation.TaxCertificateNumber = GenerateTaxCertificateNumber();

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Confirmation), new { id = donation.DonationId });
        }

        // GET: /Donation/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }

        private static string GenerateTaxCertificateNumber()
        {
            // Placeholder format — real numbering scheme can replace this later.
            return $"GOTG-TAX-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
    }
}