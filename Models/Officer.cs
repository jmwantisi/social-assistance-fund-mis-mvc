using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace socialAssistanceFundMIS.Models
{
    public class Officer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? MiddleName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public int DesignationId { get; set; }
        public Designation? Designation { get; set; }

        public bool Removed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
    }

}
