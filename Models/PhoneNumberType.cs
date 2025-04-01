using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace socialAssistanceFundMIS.Models
{
    public class PhoneNumberType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public bool Removed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
    }

}
