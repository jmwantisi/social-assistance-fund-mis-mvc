using socialAssistanceFundMIS.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialAssistanceFundMisMcv.ViewModels
{
    public class ApplicantViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Sex is required")]
        public int SexId { get; set; }
        public IEnumerable<Sex>? Sexes { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Marital Status is required")]
        public int? MaritalStatusId { get; set; }
        public IEnumerable<MaritalStatus>? MaritalStatuses { get; set; }

        public int? CountyId { get; set; }
        public int? SubCountyId { get; set; }
        public int? LocationId { get; set; }
        public int? SubLocationId { get; set; }
        public int? VillageId { get; set; }

        public IEnumerable<GeographicLocation>? Counties { get; set; }
        public IEnumerable<GeographicLocation>? SubCounties { get; set; }
        public IEnumerable<GeographicLocation>? Locations { get; set; }
        public IEnumerable<GeographicLocation>? SubLocations { get; set; }
        public IEnumerable<GeographicLocation>? Villages { get; set; }

        [Required(ErrorMessage = "Identity Card Number is required")]
        public string? IdentityCardNumber { get; set; }

        public IEnumerable<ApplicantPhoneNumber>? PhoneNumbers { get; set; }

        public string? PostalAddress { get; set; }

        [Required(ErrorMessage = "Physical Address is required")]
        public string? PhysicalAddress { get; set; }

        public IEnumerable<Application>? Applications { get; set; }
    }
}
