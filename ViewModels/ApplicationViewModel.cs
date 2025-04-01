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

        [Required]
        [Display(Name = "Applicant")]
        public int applicantId { get; set; }
        public IEnumerable<Applicant>? Applicant { get; set; } // List of Created Applicants from DB

        [Required]
        [Display(Name = "Program")]
        public int SelectedProgramId { get; set; }
        public IEnumerable<AssistanceProgram>? Programs { get; set; } // List of programs from DB

        [Required]
        [Display(Name = "Sex")]
        public int SelectedSexId { get; set; }
        public IEnumerable<Sex>? Sexes { get; set; } // List of sex options from DB

        [Required]
        [Display(Name = "Marital Status")]
        public int SelectedMaritalStatusId { get; set; }
        public IEnumerable<MaritalStatus>? MaritalStatuses { get; set; } // List of marital statuses from DB

        [Display(Name = "Status")]
        public int statusId { get; set; }
        public IEnumerable<Status>? Statuses { get; set; }
    }
}
