using Newtonsoft.Json.Linq;
using HMS.Data;
using HMS.Entity;
namespace HMS.Business;
public interface ICustomerService{
    // Define methods for customer operations
    public JObject GetCustomers(int limit);
    public JObject GetCustomer(int id);
    public JObject CreateCustomer(CreateCustomerEntity entity);
    public JObject UpdateCustomer(UpdateCustomerEntity entity);
    public JObject DeleteCustomer(int id);
}

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public JObject GetCustomers(int limit)
    {
        // Implementation for retrieving customers
        return new JObject { ["Message"] = $"Retrieved {limit} customers." };
    }

    public JObject GetCustomer(int id)
    {
        // Implementation for retrieving a single 
        object obj= _customerRepository.GetCustomerById(id);
        return new JObject { ["Message"] = $"Retrieved customer with ID {id}.", ["Data"]=JObject.FromObject(obj) };
    }

    public JObject CreateCustomer(CreateCustomerEntity entity)
    {
        // Implementation for creating a customer
        return new JObject { ["Message"] = $"Created customer: {entity.FirstName} {entity.LastName}." };
    }

    public JObject UpdateCustomer(UpdateCustomerEntity entity)
    {
        // Implementation for updating a customer
        return new JObject { ["Message"] = $"Updated customer with ID {entity.CustomerId}." };
    }

    public JObject DeleteCustomer(int id)
    {
        // Implementation for deleting a customer
        return new JObject { ["Message"] = $"Deleted customer with ID {id}." };
    }
}