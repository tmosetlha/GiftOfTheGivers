using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class ReliefProject
    {
        [Key]
        public int ReliefProjectId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        // Nullable — project may still be active/open-ended.
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active / Completed / Suspended

        // Navigation — "many" side entities that reference this project.
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
        public ICollection<ReliefUpdate> ReliefUpdates { get; set; } = new List<ReliefUpdate>();
    }
}