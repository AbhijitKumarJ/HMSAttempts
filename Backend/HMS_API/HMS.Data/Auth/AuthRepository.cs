using HMS.Data.DBModel;

namespace HMS.Data.Auth;

public interface IAuthRepository
{
    object GetUserById(int id);
}

public class AuthRepository : IAuthRepository
{
    private readonly HMSContext _context;

    public AuthRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserId == id);
        return user ?? new object();
    }
}