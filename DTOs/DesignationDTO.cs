public class DesignationDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool Removed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; }
}
