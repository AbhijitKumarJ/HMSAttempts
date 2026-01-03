using HMS.Data.Bus;
using HMS.Entity.Bus;
using Newtonsoft.Json.Linq;

namespace HMS.Business.Bus;

public interface IBusService
{
    JObject GetUsers(int limit);
    JObject GetUser(int id);
    JObject CreateUser(CreateUserEntity entity);
    JObject UpdateUser(UpdateUserEntity entity);
    JObject DeleteUser(int id);
}

public class BusService : IBusService
{
    private readonly IBusRepository _busRepository;

    public BusService(IBusRepository busRepository)
    {
        _busRepository = busRepository;
    }
    public JObject GetUsers(int limit)
    {
        return new JObject { ["Message"] = $"Retrieved {limit} users from Bus service." };
    }

    public JObject GetUser(int id)
    {
        var userData = _busRepository.GetUserById(id);
        return new JObject 
        { 
            ["Message"] = $"Retrieved user with ID {id} from Bus service.",
            ["Data"] = JObject.FromObject(userData)
        };
    }

    public JObject CreateUser(CreateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Created user: {entity.Username} in Bus service." };
    }

    public JObject UpdateUser(UpdateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in Bus service." };
    }

    public JObject DeleteUser(int id)
    {
        return new JObject { ["Message"] = $"Deleted user with ID {id} from Bus service." };
    }
}