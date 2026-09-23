using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GiftOfTheGivers.Models
{
    // Extends ASP.NET Identity's IdentityUser (table: AspNetUsers).
    // Id, Email/UserName, PhoneNumber, and PasswordHash are already provided
    // by IdentityUser — only FullName needs to be added per Section B, 2.2.2.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        // Navigation properties — the "1" side of each 1-to-many relationship.
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
        public ICollection<Volunteer> VolunteerRegistrations { get; set; } = new List<Volunteer>();
        public ICollection<ReliefUpdate> PostedUpdates { get; set; } = new List<ReliefUpdate>();
    }
}