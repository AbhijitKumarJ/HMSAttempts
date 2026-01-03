using HMS.Data.Cpoe;
using HMS.Entity.Cpoe;
using Newtonsoft.Json.Linq;

namespace HMS.Business.Cpoe;

public interface ICpoeService
{
    JObject GetUsers(int limit);
    JObject GetUser(int id);
    JObject CreateUser(CreateUserEntity entity);
    JObject UpdateUser(UpdateUserEntity entity);
    JObject DeleteUser(int id);
}

public class CpoeService : ICpoeService
{
    private readonly ICpoeRepository _cpoeRepository;

    public CpoeService(ICpoeRepository cpoeRepository)
    {
        _cpoeRepository = cpoeRepository;
    }
    public JObject GetUsers(int limit)
    {
        return new JObject { ["Message"] = $"Retrieved {limit} users from CPOE service." };
    }

    public JObject GetUser(int id)
    {
        var userData = _cpoeRepository.GetUserById(id);
        return new JObject 
        { 
            ["Message"] = $"Retrieved user with ID {id} from CPOE service.",
            ["Data"] = JObject.FromObject(userData)
        };
    }

    public JObject CreateUser(CreateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Created user: {entity.Username} in CPOE service." };
    }

    public JObject UpdateUser(UpdateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in CPOE service." };
    }

    public JObject DeleteUser(int id)
    {
        return new JObject { ["Message"] = $"Deleted user with ID {id} from CPOE service." };
    }
}