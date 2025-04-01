using socialAssistanceFundMIS.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace socialAssistanceFundMIS.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        public DateTime ApplicationDate { get; set; }

        public int ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }

        public int ProgramId { get; set; }
        public AssistanceProgram? Program { get; set; }

        public int StatusId { get; set; } = 0;  // Pending
        public Status? Status { get; set; }

        public int OfficialRecordId { get; set; }
        public OfficialRecord? OfficialRecord { get; set; }

        public DateTime DeclarationDate { get; set; }

        public bool Removed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }
    }

}
