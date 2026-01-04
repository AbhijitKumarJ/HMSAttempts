using HMS.Data.DBModel;

namespace HMS.Data.Billing;

public interface IBillingRepository
{
    object GetUserById(int id);
}

public class BillingRepository : IBillingRepository
{
    private readonly HMSContext _context;

    public BillingRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user ?? new object();
    }
}