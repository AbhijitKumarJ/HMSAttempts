using HMS.Data.Inventory;
using HMS.Entity.Inventory;
using Newtonsoft.Json.Linq;

namespace HMS.Business.Inventory;

public interface IInventoryService
{
    JObject GetUsers(int limit);
    JObject GetUser(int id);
    JObject CreateUser(CreateUserEntity entity);
    JObject UpdateUser(UpdateUserEntity entity);
    JObject DeleteUser(int id);
}

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    public JObject GetUsers(int limit)
    {
        return new JObject { ["Message"] = $"Retrieved {limit} users from Inventory service." };
    }

    public JObject GetUser(int id)
    {
        var userData = _inventoryRepository.GetUserById(id);
        return new JObject 
        { 
            ["Message"] = $"Retrieved user with ID {id} from Inventory service.",
            ["Data"] = JObject.FromObject(userData)
        };
    }

    public JObject CreateUser(CreateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Created user: {entity.Username} in Inventory service." };
    }

    public JObject UpdateUser(UpdateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in Inventory service." };
    }

    public JObject DeleteUser(int id)
    {
        return new JObject { ["Message"] = $"Deleted user with ID {id} from Inventory service." };
    }
}