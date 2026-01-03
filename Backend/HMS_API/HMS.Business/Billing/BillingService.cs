using HMS.Data.Billing;
using HMS.Entity.Billing;
using Newtonsoft.Json.Linq;

namespace HMS.Business.Billing;

public interface IBillingService
{
    JObject GetUsers(int limit);
    JObject GetUser(int id);
    JObject CreateUser(CreateUserEntity entity);
    JObject UpdateUser(UpdateUserEntity entity);
    JObject DeleteUser(int id);
}

public class BillingService : IBillingService
{
    private readonly IBillingRepository _billingRepository;

    public BillingService(IBillingRepository billingRepository)
    {
        _billingRepository = billingRepository;
    }
    public JObject GetUsers(int limit)
    {
        return new JObject { ["Message"] = $"Retrieved {limit} users from Billing service." };
    }

    public JObject GetUser(int id)
    {
        var userData = _billingRepository.GetUserById(id);
        return new JObject 
        { 
            ["Message"] = $"Retrieved user with ID {id} from Billing service.",
            ["Data"] = JObject.FromObject(userData)
        };
    }

    public JObject CreateUser(CreateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Created user: {entity.Username} in Billing service." };
    }

    public JObject UpdateUser(UpdateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in Billing service." };
    }

    public JObject DeleteUser(int id)
    {
        return new JObject { ["Message"] = $"Deleted user with ID {id} from Billing service." };
    }
}