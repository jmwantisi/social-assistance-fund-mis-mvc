using System.ComponentModel.DataAnnotations;

public class ApplicantDTO
{
    public int Id { get; set; }

    [Required]
    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    public int SexId { get; set; }
    public string? SexName { get; set; }  // Flattened field for the sex name

    [Required]
    public DateTime Dob { get; set; }

    [Required]
    public int? MaritialStatusId { get; set; }
    public string? MaritialStatusName { get; set; }  // Flattened field for the marital status name

    public int? CountyId { get; set; }
    public string? CountyName { get; set; }  // Flattened field for county name

    public int? SubCountyId { get; set; }
    public string? SubCountyName { get; set; }  // Flattened field for sub-county name

    public int? LocationId { get; set; }
    public string? LocationName { get; set; }  // Flattened field for location name

    public int? SubLocationId { get; set; }
    public string? SubLocationName { get; set; }  // Flattened field for sub-location name

    public int? VillageId { get; set; }
    public string? VillageName { get; set; }  // Flattened field for village name

    [Required]
    public string? IdentityCardNumber { get; set; }

    public List<ApplicantPhoneNumberDTO> PhoneNumbers { get; set; } = new();  // Use PhoneNumberDTO for phone numbers

    public string? PostalAddress { get; set; }

    [Required]
    public string? PhysicalAddress { get; set; }

    public bool Removed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}
