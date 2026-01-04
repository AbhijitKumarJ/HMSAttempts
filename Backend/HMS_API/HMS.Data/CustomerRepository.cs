using HMS.Data.DBModel;

namespace HMS.Data;

public interface IUserRepository
{
    // Define methods for user data access
    public object GetUserById(int id);
}

public class UserRepository : IUserRepository
{
    private readonly HMSContext _context;

    public UserRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        System.Console.WriteLine("Fetching user by ID from database..." + id);
        var user = _context.Users.FirstOrDefault(c => c.Id == id);
        // Implementation for retrieving a user by ID from the database
        //return new { CustomerId = id, FirstName = "John", LastName = "Doe" };

        return user??new object();
    }
}