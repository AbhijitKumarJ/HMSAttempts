using HMS.Data.DBModel;

namespace HMS.Data.Inventory;

public interface IInventoryRepository
{
    object GetUserById(int id);
}

public class InventoryRepository : IInventoryRepository
{
    private readonly HMSContext _context;

    public InventoryRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user ?? new object();
    }
}