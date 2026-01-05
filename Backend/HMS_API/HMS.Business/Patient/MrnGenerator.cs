using Microsoft.EntityFrameworkCore;
using HMS.Data.DBModel;

namespace HMS.Business.Patient;

public interface IMrnGenerator
{
    Task<string> GenerateMrnAsync();
}

public class MrnGenerator : IMrnGenerator
{
    private readonly HMSContext _context;

    public MrnGenerator(HMSContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateMrnAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"MRN-{year}-";

        var lastMrn = await _context.PatPatients
            .Where(p => p.Mrn != null && p.Mrn.StartsWith(prefix))
            .OrderByDescending(p => p.Mrn)
            .Select(p => p.Mrn)
            .FirstOrDefaultAsync();

        int sequence = 1;
        if (lastMrn != null)
        {
            var sequencePart = lastMrn.Substring(prefix.Length);
            if (int.TryParse(sequencePart, out var lastSequence))
            {
                sequence = lastSequence + 1;
            }
        }

        return $"{prefix}{sequence:D4}";
    }
}
