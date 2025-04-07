public class GeographicLocationDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int GeographicLocationTypeId { get; set; }

    public int GeographicLocationParentId { get; set; }

    public bool Removed { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Optional: You can include related data if needed
    public GeographicLocationTypeDTO? GeographicLocationType { get; set; }
    public GeographicLocationDTO? ParentLocation { get; set; }
}
