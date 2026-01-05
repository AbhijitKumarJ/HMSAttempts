namespace HMS.Entity.Patient;

public class UpdatePatientDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Gender { get; set; }
    public ContactInfoDto? ContactInfo { get; set; }
}

public class ContactInfoDto
{
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? InsuranceProvider { get; set; }
    public string? PolicyNumber { get; set; }
}
