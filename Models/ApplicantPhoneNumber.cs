using socialAssistanceFundMIS.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace socialAssistanceFundMIS.Models
{
    public class ApplicantPhoneNumber
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public int PhoneNumberTypeId { get; set; }

        public PhoneNumberType? PhoneNumberType { get; set; }

        public bool Removed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
    }

}
