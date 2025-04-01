using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace socialAssistanceFundMIS.Models
{
    public class GeographicLocation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int GeographicLocationTypeId { get; set; }
        public GeographicLocationType? GeographicLocationType { get; set; }

        public int? GeographicLocationParentId { get; set; }
        public GeographicLocation? GeographicLocationParent { get; set; }

        public bool Removed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
