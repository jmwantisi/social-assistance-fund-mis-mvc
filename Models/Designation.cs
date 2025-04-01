using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace socialAssistanceFundMIS.Models
{
    public class Designation
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool Removed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
    }
}
