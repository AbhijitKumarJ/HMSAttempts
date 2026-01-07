namespace HMS.Entity.Patient;

public class PatientSummaryDto
{
    public DemographicsDto Demographics { get; set; } = null!;
    public VitalsRibbonDto VitalsRibbon { get; set; } = null!;
    public List<AllergyDto> Allergies { get; set; } = new();
    public List<ActiveProblemDto> ActiveProblems { get; set; } = new();
    public List<TimelineEventDto> RecentTimelineEvents { get; set; } = new();
}

public class DemographicsDto
{
    public int Id { get; set; }
    public string Mrn { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Gender { get; set; }
    public DateOnly? Dob { get; set; }
    public string? Age { get; set; }
    public bool HasAllergies { get; set; }
}

public class VitalsRibbonDto
{
    public DateTime? RecordedAt { get; set; }
    public int? BpSystolic { get; set; }
    public int? BpDiastolic { get; set; }
    public int? HeartRate { get; set; }
    public decimal? Temperature { get; set; }
    public int? Spo2 { get; set; }
}

public class AllergyDto
{
    public string Name { get; set; } = null!;
    public string? Severity { get; set; }
    public string? Reaction { get; set; }
}

public class ActiveProblemDto
{
    public string Name { get; set; } = null!;
    public DateTime? OnsetDate { get; set; }
    public string? Status { get; set; }
}

public class TimelineEventDto
{
    public string Type { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public string? Summary { get; set; }
    public string? Details { get; set; }
    public string? PerformedBy { get; set; }
}