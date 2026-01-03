using HMS.Entity.Auth;
using HMS.Data.Auth;
using Newtonsoft.Json.Linq;

namespace HMS.Business.Auth;

public interface IAuthService
{
    JObject GetUsers(int limit);
    JObject GetUser(int id);
    JObject CreateUser(CreateUserEntity entity);
    JObject UpdateUser(UpdateUserEntity entity);
    JObject DeleteUser(int id);
}

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public JObject GetUsers(int limit)
    {
        return new JObject { ["Message"] = $"Retrieved {limit} users from Auth service." };
    }

    public JObject GetUser(int id)
    {
        var user = _authRepository.GetUserById(id);
        return new JObject { ["Message"] = $"Retrieved user with ID {id} from Auth service.", ["Data"] = JObject.FromObject(user) };
    }

    public JObject CreateUser(CreateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Created user: {entity.Username} in Auth service." };
    }

    public JObject UpdateUser(UpdateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in Auth service." };
    }

    public JObject DeleteUser(int id)
    {
        return new JObject { ["Message"] = $"Deleted user with ID {id} from Auth service." };
    }
}