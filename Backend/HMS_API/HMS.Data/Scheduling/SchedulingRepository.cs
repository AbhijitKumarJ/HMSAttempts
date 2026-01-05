using HMS.Data.DBModel;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Scheduling;

public interface ISchedulingRepository
{
    Task<SchAppointment?> GetByIdAsync(long id);
    Task<List<SchAppointment>> GetByDoctorIdAsync(int doctorId);
    Task<List<SchAppointment>> SearchAppointmentsAsync(DateTime startDate, DateTime? endDate, int? doctorId);
    Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate);
    Task<SchAppointment> CreateAsync(SchAppointment appointment);
    Task<SchAppointment> UpdateAsync(SchAppointment appointment);
}

public class SchedulingRepository : ISchedulingRepository
{
    private readonly HMSContext _context;

    public SchedulingRepository(HMSContext context)
    {
        _context = context;
    }

    public async Task<SchAppointment?> GetByIdAsync(long id)
    {
        return await _context.SchAppointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<SchAppointment>> GetByDoctorIdAsync(int doctorId)
    {
        return await _context.SchAppointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.DoctorId == doctorId)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<List<SchAppointment>> SearchAppointmentsAsync(DateTime startDate, DateTime? endDate, int? doctorId)
    {
        var query = _context.SchAppointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .AsQueryable();

        var startOfDay = startDate.Date;
        var endOfDay = endDate.HasValue ? endDate.Value.Date.AddDays(1).AddTicks(-1) : startOfDay.AddDays(1).AddTicks(-1);

        query = query.Where(a => a.AppointmentDate >= startOfDay && a.AppointmentDate <= endOfDay);

        if (doctorId.HasValue)
        {
            query = query.Where(a => a.DoctorId == doctorId);
        }

        return await query
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
    }

    public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate)
    {
        var existingAppointment = await _context.SchAppointments
            .FirstOrDefaultAsync(a => 
                a.DoctorId == doctorId &&
                a.AppointmentDate == appointmentDate &&
                a.Status != "Cancelled");

        return existingAppointment == null;
    }

    public async Task<SchAppointment> CreateAsync(SchAppointment appointment)
    {
        _context.SchAppointments.Add(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }

    public async Task<SchAppointment> UpdateAsync(SchAppointment appointment)
    {
        _context.SchAppointments.Update(appointment);
        await _context.SaveChangesAsync();
        return appointment;
    }
}
