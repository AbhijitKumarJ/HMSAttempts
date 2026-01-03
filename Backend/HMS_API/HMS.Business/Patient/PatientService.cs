using HMS.Data.Patient;
using HMS.Entity.Patient;
using Newtonsoft.Json.Linq;

namespace HMS.Business.Patient;

public interface IPatientService
{
    JObject GetUsers(int limit);
    JObject GetUser(int id);
    JObject CreateUser(CreateUserEntity entity);
    JObject UpdateUser(UpdateUserEntity entity);
    JObject DeleteUser(int id);
}

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
    public JObject GetUsers(int limit)
    {
        return new JObject { ["Message"] = $"Retrieved {limit} users from Patient service." };
    }

    public JObject GetUser(int id)
    {
        var userData = _patientRepository.GetUserById(id);
        return new JObject 
        { 
            ["Message"] = $"Retrieved user with ID {id} from Patient service.",
            ["Data"] = JObject.FromObject(userData)
        };
    }

    public JObject CreateUser(CreateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Created user: {entity.Username} in Patient service." };
    }

    public JObject UpdateUser(UpdateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in Patient service." };
    }

    public JObject DeleteUser(int id)
    {
        return new JObject { ["Message"] = $"Deleted user with ID {id} from Patient service." };
    }
}