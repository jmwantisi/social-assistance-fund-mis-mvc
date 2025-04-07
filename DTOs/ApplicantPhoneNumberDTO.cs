public class ApplicantPhoneNumberDTO
{
    public int Id { get; set; }
    public string? PhoneNumber { get; set; }
    public int PhoneNumberTypeId { get; set; }
    public string? PhoneNumberType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}