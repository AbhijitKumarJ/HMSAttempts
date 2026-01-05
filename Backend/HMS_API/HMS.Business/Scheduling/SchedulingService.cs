using HMS.Data.Scheduling;
using HMS.Data.Patient;
using HMS.Data.DBModel;
using HMS.Entity.Scheduling;

namespace HMS.Business.Scheduling;

public interface ISchedulingService
{
    Task<AppointmentResponseDto> BookAppointmentAsync(BookAppointmentDto dto);
    Task<AppointmentResponseDto?> GetAppointmentByIdAsync(long id);
    Task<List<AppointmentResponseDto>> SearchAppointmentsAsync(DateTime startDate, DateTime? endDate, int? doctorId);
}

public class SchedulingService : ISchedulingService
{
    private readonly ISchedulingRepository _schedulingRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly HMSContext _context;

    public SchedulingService(
        ISchedulingRepository schedulingRepository,
        IPatientRepository patientRepository,
        HMSContext context)
    {
        _schedulingRepository = schedulingRepository;
        _patientRepository = patientRepository;
        _context = context;
    }

    public async Task<AppointmentResponseDto> BookAppointmentAsync(BookAppointmentDto dto)
    {
        var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
        if (patient == null)
        {
            throw new ArgumentException($"Patient with ID {dto.PatientId} not found");
        }

        var doctor = await _context.Users.FindAsync(dto.DoctorId);
        if (doctor == null)
        {
            throw new ArgumentException($"Doctor with ID {dto.DoctorId} not found");
        }

        var isAvailable = await _schedulingRepository.IsDoctorAvailableAsync(
            dto.DoctorId, 
            dto.AppointmentDate);

        if (!isAvailable)
        {
            throw new InvalidOperationException($"Doctor {doctor.Username} is not available at {dto.AppointmentDate}");
        }

        var appointment = new SchAppointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            AppointmentDate = dto.AppointmentDate,
            ReasonForVisit = dto.ReasonForVisit,
            Status = "Scheduled",
            CreatedAt = DateTime.UtcNow
        };

        var createdAppointment = await _schedulingRepository.CreateAsync(appointment);

        return new AppointmentResponseDto
        {
            Id = createdAppointment.Id,
            PatientId = createdAppointment.PatientId!.Value,
            DoctorId = createdAppointment.DoctorId!.Value,
            AppointmentDate = createdAppointment.AppointmentDate!.Value,
            Status = createdAppointment.Status!,
            ReasonForVisit = createdAppointment.ReasonForVisit,
            CreatedAt = createdAppointment.CreatedAt!.Value,
            PatientName = $"{patient.FirstName} {patient.LastName}",
            DoctorName = doctor.Username
        };
    }

    public async Task<AppointmentResponseDto?> GetAppointmentByIdAsync(long id)
    {
        var appointment = await _schedulingRepository.GetByIdAsync(id);
        if (appointment == null)
        {
            return null;
        }

        return new AppointmentResponseDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId!.Value,
            DoctorId = appointment.DoctorId!.Value,
            AppointmentDate = appointment.AppointmentDate!.Value,
            Status = appointment.Status!,
            ReasonForVisit = appointment.ReasonForVisit,
            CreatedAt = appointment.CreatedAt!.Value,
            PatientName = $"{appointment.Patient?.FirstName} {appointment.Patient?.LastName}",
            DoctorName = appointment.Doctor?.Username ?? "Unknown"
        };
    }

    public async Task<List<AppointmentResponseDto>> SearchAppointmentsAsync(DateTime startDate, DateTime? endDate, int? doctorId)
    {
        var appointments = await _schedulingRepository.SearchAppointmentsAsync(startDate, endDate, doctorId);

        return appointments.Select(a => new AppointmentResponseDto
        {
            Id = a.Id,
            PatientId = a.PatientId!.Value,
            DoctorId = a.DoctorId!.Value,
            AppointmentDate = a.AppointmentDate!.Value,
            Status = a.Status!,
            ReasonForVisit = a.ReasonForVisit,
            CreatedAt = a.CreatedAt!.Value,
            PatientName = $"{a.Patient?.FirstName} {a.Patient?.LastName}",
            DoctorName = a.Doctor?.Username ?? "Unknown"
        }).ToList();
    }
}
