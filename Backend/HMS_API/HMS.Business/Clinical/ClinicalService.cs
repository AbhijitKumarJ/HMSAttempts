using HMS.Data.Clinical;
using HMS.Entity.Clinical;
using Newtonsoft.Json.Linq;

namespace HMS.Business.Clinical;

public interface IClinicalService
{
    JObject GetUsers(int limit);
    JObject GetUser(int id);
    JObject CreateUser(CreateUserEntity entity);
    JObject UpdateUser(UpdateUserEntity entity);
    JObject DeleteUser(int id);
}

public class ClinicalService : IClinicalService
{
    private readonly IClinicalRepository _clinicalRepository;

    public ClinicalService(IClinicalRepository clinicalRepository)
    {
        _clinicalRepository = clinicalRepository;
    }
    public JObject GetUsers(int limit)
    {
        return new JObject { ["Message"] = $"Retrieved {limit} users from Clinical service." };
    }

    public JObject GetUser(int id)
    {
        var userData = _clinicalRepository.GetUserById(id);
        return new JObject 
        { 
            ["Message"] = $"Retrieved user with ID {id} from Clinical service.",
            ["Data"] = JObject.FromObject(userData)
        };
    }

    public JObject CreateUser(CreateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Created user: {entity.Username} in Clinical service." };
    }

    public JObject UpdateUser(UpdateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in Clinical service." };
    }

    public JObject DeleteUser(int id)
    {
        return new JObject { ["Message"] = $"Deleted user with ID {id} from Clinical service." };
    }
}