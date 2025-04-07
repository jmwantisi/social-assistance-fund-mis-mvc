using System.ComponentModel.DataAnnotations;

public class OfficerDTO
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string MiddleName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public int DesignationId { get; set; }

    public bool Removed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; }
}
