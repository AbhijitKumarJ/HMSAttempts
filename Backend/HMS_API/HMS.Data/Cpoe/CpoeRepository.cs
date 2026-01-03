using HMS.Data.DBModel;

namespace HMS.Data.Cpoe;

public interface ICpoeRepository
{
    object GetUserById(int id);
}

public class CpoeRepository : ICpoeRepository
{
    private readonly HMSContext _context;

    public CpoeRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        return user ?? new object();
    }
}