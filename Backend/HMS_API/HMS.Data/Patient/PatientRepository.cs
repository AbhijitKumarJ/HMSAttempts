using HMS.Data.DBModel;

namespace HMS.Data.Patient;

public interface IPatientRepository
{
    object GetUserById(int id);
}

public class PatientRepository : IPatientRepository
{
    private readonly HMSContext _context;

    public PatientRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user ?? new object();
    }
}