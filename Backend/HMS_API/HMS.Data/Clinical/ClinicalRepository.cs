using HMS.Data.DBModel;

namespace HMS.Data.Clinical;

public interface IClinicalRepository
{
    object GetUserById(int id);
}

public class ClinicalRepository : IClinicalRepository
{
    private readonly HMSContext _context;

    public ClinicalRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user ?? new object();
    }
}