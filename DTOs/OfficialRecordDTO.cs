using System.ComponentModel.DataAnnotations;

public class OfficialRecordDTO
{
    public int Id { get; set; }

    [Required]
    public int OfficerId { get; set; }

    public DateTime OfficiationDate { get; set; }

    public bool Removed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; }
}
