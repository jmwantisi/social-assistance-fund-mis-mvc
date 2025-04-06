using socialAssistanceFundMIS.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialAssistanceFundMisMcv.ViewModels
{
    public class ApplicantViewModel
    {
        public int Id { get; set; }

        public string? FullName => $"{FirstName} {MiddleName} {LastName}".Trim();

        [Required(ErrorMessage = "First Name is required")]
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        public string? SexName { get; set; }

        [Required(ErrorMessage = "Sex is required")]
        public int SexId { get; set; }
        public IEnumerable<Sex>? Sexes { get; set; }


        public int Age => CalculateAge(Dob);

        public string? IdNumber { get; set; }

        public string? Location { get; set; }

        public string FormattedDob => GetFormattedDob(Dob);

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateOnly Dob { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddYears(-18));

        private string GetFormattedDob(DateOnly date)
        {
            string daySuffix = GetDaySuffix(date.Day);
            return $"{date.Day}{daySuffix}, {date.ToString("MMMM")}, {date.Year}";
        }

        private string GetDaySuffix(int day)
        {
            return (day % 10 == 1 && day != 11) ? "st" :
                   (day % 10 == 2 && day != 12) ? "nd" :
                   (day % 10 == 3 && day != 13) ? "rd" : "th";
        }

        public string? MaritalStatusName { get; set; }

        [Required(ErrorMessage = "Marital Status is required")]
        public int? MaritalStatusId { get; set; }
        public IEnumerable<MaritalStatus>? MaritalStatuses { get; set; }

        public int? VillageId { get; set; }
        public IEnumerable<GeographicLocation>? Villages { get; set; }

        [Required(ErrorMessage = "Identity Card Number is required")]
        [MaxLength(20, ErrorMessage = "Identity Card Number cannot exceed 20 characters.")]
        public string? IdentityCardNumber { get; set; }

        public string? PhoneNumbersListString { get; set; }

        public IEnumerable<ApplicantPhoneNumber>? PhoneNumbers { get; set; } = new List<ApplicantPhoneNumber>();

        public int? PhoneNumberTypesId { get; set; }
        public IEnumerable<PhoneNumberType>? PhoneNumberTypes { get; set; } = new List<PhoneNumberType>();

        public string? PostalAddress { get; set; }

        [Required(ErrorMessage = "Physical Address is required")]
        public string? PhysicalAddress { get; set; }

        public IEnumerable<Application>? Applications { get; set; }

        private int CalculateAge(DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - dob.Year;

            // Adjust age if birthday hasn't occurred yet this year
            if (today < new DateOnly(today.Year, dob.Month, dob.Day))
            {
                age--;
            }

            return age;
        }

    }
}
