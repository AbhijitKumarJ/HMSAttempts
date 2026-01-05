namespace HMS.Entity.Patient;

public class PatientSearchResultDto
{
    public int Id { get; set; }
    public string Mrn { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
}
