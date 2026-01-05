namespace HMS.Entity.Patient;

public class PatientResponseDto
{
    public int Id { get; set; }
    public string Mrn { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public ContactInfoDto? ContactInfo { get; set; }
    public bool IsEmergencyReg { get; set; }
    public DateTime CreatedAt { get; set; }
}
