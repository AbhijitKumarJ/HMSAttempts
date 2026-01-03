using HMS.Data.DBModel;

namespace HMS.Data;

public interface ICustomerRepository
{
    // Define methods for customer data access
    public object GetCustomerById(int id);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly DvdRentalContext _context;

    public CustomerRepository(DvdRentalContext context)
    {
        _context = context;
    }

    public object GetCustomerById(int id)
    {
        System.Console.WriteLine("Fetching customer by ID from database..." + id);
        var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
        // Implementation for retrieving a customer by ID from the database
        //return new { CustomerId = id, FirstName = "John", LastName = "Doe" };

        return customer??new object();
    }
}