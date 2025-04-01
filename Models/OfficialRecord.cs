using socialAssistanceFundMIS.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace socialAssistanceFundMIS.Models
{
    public class OfficialRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OfficerId { get; set; }
        public Officer? Officer { get; set; }

        [Required]
        public DateTime OfficiationDate { get; set; }

        public bool Removed { get; set; } = false; // Using boolean for clarity

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } // Consider updating this manually
    }
}
