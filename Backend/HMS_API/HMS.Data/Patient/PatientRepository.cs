using HMS.Data.DBModel;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Patient;

public interface IPatientRepository
{
    Task<PatPatient?> GetByMrnAsync(string mrn);
    Task<PatPatient?> GetByIdAsync(int id);
    Task<PatPatient> CreateAsync(PatPatient patient);
    Task<PatPatient> UpdateAsync(PatPatient patient);
    Task<List<PatPatient>> SearchPatientsAsync(string query);
    Task<List<PatPatient>> GetAllPatientsAsync();
    Task<(PatPatient? patient, ClinVital? latestVital, List<ClinConsultation> consultations, List<SchAppointment> appointments, List<LabResult> labResults)> GetPatientSummaryDataAsync(string mrn);
}

public class PatientRepository : IPatientRepository
{
    private readonly HMSContext _context;

    public PatientRepository(HMSContext context)
    {
        _context = context;
    }
    
    public async Task<PatPatient?> GetByMrnAsync(string mrn)
    {
        return await _context.PatPatients
            .FirstOrDefaultAsync(p => p.Mrn == mrn);
    }

    public async Task<PatPatient?> GetByIdAsync(int id)
    {
        return await _context.PatPatients
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PatPatient> CreateAsync(PatPatient patient)
    {
        _context.PatPatients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<PatPatient> UpdateAsync(PatPatient patient)
    {
        _context.PatPatients.Update(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<List<PatPatient>> SearchPatientsAsync(string query)
    {
        var searchTerm = $"%{query}%";
        
        return await _context.PatPatients
            .Where(p => EF.Functions.ILike(p.Mrn, searchTerm) ||
                       EF.Functions.ILike(p.FirstName!, searchTerm) ||
                       EF.Functions.ILike(p.LastName!, searchTerm))
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<PatPatient>> GetAllPatientsAsync()
    {
        return await _context.PatPatients
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();
    }

    public async Task<(PatPatient? patient, ClinVital? latestVital, List<ClinConsultation> consultations, List<SchAppointment> appointments, List<LabResult> labResults)> GetPatientSummaryDataAsync(string mrn)
    {
        var patient = await _context.PatPatients
            .FirstOrDefaultAsync(p => p.Mrn == mrn);

        if (patient == null)
        {
            return (null, null, new List<ClinConsultation>(), new List<SchAppointment>(), new List<LabResult>());
        }

        var latestVital = await _context.ClinVitals
            .Where(v => v.PatientId == patient.Id)
            .OrderByDescending(v => v.RecordedAt)
            .FirstOrDefaultAsync();

        var consultations = await _context.ClinConsultations
            .Where(c => c.PatientId == patient.Id && c.StartedAt.HasValue)
            .OrderByDescending(c => c.StartedAt)
            .Take(10)
            .ToListAsync();

        var appointments = await _context.SchAppointments
            .Include(a => a.Doctor)
            .Where(a => a.PatientId == patient.Id)
            .OrderByDescending(a => a.AppointmentDate)
            .Take(10)
            .ToListAsync();

        var labResults = await _context.LabResults
            .Include(l => l.Order)
            .Where(l => l.Order != null && l.Order.PatientId == patient.Id)
            .OrderByDescending(l => l.ReleasedAt)
            .Take(10)
            .ToListAsync();

        return (patient, latestVital, consultations, appointments, labResults);
    }
}