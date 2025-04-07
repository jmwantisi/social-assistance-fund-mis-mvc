public class ApplicationDTO
{
    public int Id { get; set; }

    public DateTime ApplicationDate { get; set; }

    public int ApplicantId { get; set; }
    public string? ApplicantFirstName { get; set; }
    public string? ApplicantMiddleName { get; set; }
    public string? ApplicantLastName { get; set; }

    public int ProgramId { get; set; }
    public string? ProgramName { get; set; }

    public int StatusId { get; set; }
    public string? StatusName { get; set; } 

    public int OfficialRecordId { get; set; }
    public string? OfficalFirstName { get; set; }
    public string? OfficialMiddleName { get; set; }
    public string? OfficialLastName { get; set; }


    public DateTime DeclarationDate { get; set; }

    public bool Removed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; }
}
