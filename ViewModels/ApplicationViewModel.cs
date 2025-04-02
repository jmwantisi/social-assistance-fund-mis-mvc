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
        public DateTime ApplicationDate { get; set; }

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
        public int statusId { get; set; }
        public IEnumerable<Status>? Statuses { get; set; }

        public Applicant? Applicant { get; set; }

        public AssistanceProgram? Program { get; set; }

        [Display(Name = "Decleration Date")]
        public DateTime DeclarationDate { get; set; }
    }
}
