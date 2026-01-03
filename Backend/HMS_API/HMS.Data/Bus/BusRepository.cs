using HMS.Data.DBModel;

namespace HMS.Data.Bus;

public interface IBusRepository
{
    object GetUserById(int id);
}

public class BusRepository : IBusRepository
{
    private readonly HMSContext _context;

    public BusRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        return user ?? new object();
    }
}