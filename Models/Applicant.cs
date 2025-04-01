using socialAssistanceFundMIS.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace socialAssistanceFundMIS.Data
{
    public class Applicant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public int SexId { get; set; }
        public Sex? Sex { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        public int? MaritialStatusId { get; set; }
        public MaritalStatus? MaritialStatus { get; set; }

        [ForeignKey("CountyId")]
        public int? CountyId { get; set; }
        public GeographicLocation? County { get; set; }

        [ForeignKey("SubCountyId")]
        public int? SubCountyId { get; set; }
        public GeographicLocation? SubCounty { get; set; }

        [ForeignKey("LocationId")]
        public int? LocationId { get; set; }
        public GeographicLocation? Location { get; set; }

        [ForeignKey("SubLocationId")]
        public int? SubLocationId { get; set; }
        public GeographicLocation? SubLocation { get; set; }

        [ForeignKey("VillageId")]
        public int? VillageId { get; set; }
        public GeographicLocation? Village { get; set; }


        [Required]
        public string? IdentityCardNumber { get; set; }

        [InverseProperty("Applicant")]
        public List<ApplicantPhoneNumber> PhoneNumbers { get; set; } = new();

        public string? PostalAddress { get; set; }

        [Required]
        public string? PhysicalAddress { get; set; }

        [InverseProperty("Applicant")]
        public List<Application>? Applications { get; set; }

        public bool Removed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}
