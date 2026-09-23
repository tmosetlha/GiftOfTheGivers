using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerId { get; set; }

        // Nullable — supports lightweight "register interest" sign-ups
        // before a full user account exists.
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Skills { get; set; } = string.Empty; // e.g. medical, logistics, driving

        [Required]
        [StringLength(20)]
        public string Availability { get; set; } = string.Empty; // Weekdays / Weekends / OnCall

        [Required]
        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        public bool IsConfirmed { get; set; }
    }
}