namespace HMS.Entity.Scheduling;

public class BookAppointmentDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? ReasonForVisit { get; set; }
}

public class AppointmentResponseDto
{
    public long Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = null!;
    public string? ReasonForVisit { get; set; }
    public DateTime CreatedAt { get; set; }
    public string PatientName { get; set; } = null!;
    public string DoctorName { get; set; } = null!;
}
