using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class ReliefUpdate
    {
        [Key]
        public int UpdateId { get; set; }

        // Required — the employee who posted the update.
        [Required]
        [ForeignKey(nameof(Employee))]
        public string EmployeeId { get; set; } = string.Empty;
        public ApplicationUser Employee { get; set; } = null!;

        // Required — the project this update belongs to.
        [Required]
        [ForeignKey(nameof(ReliefProject))]
        public int ReliefProjectId { get; set; }
        public ReliefProject ReliefProject { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;
    }
}