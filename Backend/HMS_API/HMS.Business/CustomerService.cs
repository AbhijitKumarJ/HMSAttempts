using Newtonsoft.Json.Linq;
using HMS.Data;
using HMS.Entity;
namespace HMS.Business;
public interface IUserService{
    // Define methods for user operations
    public JObject GetUsers(int limit);
    public JObject GetUser(int id);
    public JObject CreateUser(CreateUserEntity entity);
    public JObject UpdateUser(UpdateUserEntity entity);
    public JObject DeleteUser(int id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public JObject GetUsers(int limit)
    {
        // Implementation for retrieving users
        return new JObject { ["Message"] = $"Retrieved {limit} users." };
    }

    public JObject GetUser(int id)
    {
        // Implementation for retrieving a single user by ID
        object obj= _userRepository.GetUserById(id);
        return new JObject { ["Message"] = $"Retrieved user with ID {id}.", ["Data"]=JObject.FromObject(obj) };
    }

    public JObject CreateUser(CreateUserEntity entity)
    {
        // Implementation for creating a user
        return new JObject { ["Message"] = $"Created user: {entity.Username}." };
    }

    public JObject UpdateUser(UpdateUserEntity entity)
    {
        // Implementation for updating a user
        return new JObject { ["Message"] = $"Updated user with ID {entity.UserId}." };
    }

    public JObject DeleteUser(int id)
    {
        // Implementation for deleting a user
        return new JObject { ["Message"] = $"Deleted user with ID {id}." };
    }
}