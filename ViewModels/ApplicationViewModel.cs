using socialAssistanceFundMIS.Data;
using socialAssistanceFundMIS.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialAssistanceFundMisMcv.ViewModels
{
    public class ApplicationViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Application Date")]
        public DateOnly ApplicationDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public string? ApplicantFullName { get; set; }

        [Required]
        [Display(Name = "Applicant")]
        public int ApplicantId { get; set; }
        public IEnumerable<Applicant>? Applicants { get; set; } // List of Created Applicants from DB

        [Required]
        [Display(Name = "Program")]
        public int SelectedProgramId { get; set; }
        public IEnumerable<AssistanceProgram>? Programs { get; set; } // List of programs from DB

        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public IEnumerable<Status>? Statuses { get; set; }

        public Applicant? Applicant { get; set; }

        public int ProgramId { get; set; }

        public AssistanceProgram? Program { get; set; }

        [Display(Name = "Declaration Date")]
        public DateOnly? DeclarationDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        public string FormattedDeclarationDate =>
            DeclarationDate.HasValue ? GetFormattedDate(DeclarationDate.Value) : string.Empty;

        private string GetFormattedDate(DateOnly date)
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
    }
}
