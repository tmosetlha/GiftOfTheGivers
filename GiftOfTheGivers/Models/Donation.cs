using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }

        // Nullable — NULL means an anonymous / guest donation.
        [ForeignKey(nameof(Donor))]
        public string? DonorId { get; set; }
        public ApplicationUser? Donor { get; set; }

        // Nullable — donation may be general (not earmarked to a specific project).
        [ForeignKey(nameof(ReliefProject))]
        public int? ReliefProjectId { get; set; }
        public ReliefProject? ReliefProject { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = "ZAR"; // ZAR / USD / EUR

        [Required]
        [StringLength(20)]
        public string DonationType { get; set; } = "OneTime"; // OneTime / Recurring

        public bool IsAnonymous { get; set; }

        [Required]
        public DateTime DonationDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending / Completed / Failed

        [StringLength(50)]
        public string? TaxCertificateNumber { get; set; }
    }
}