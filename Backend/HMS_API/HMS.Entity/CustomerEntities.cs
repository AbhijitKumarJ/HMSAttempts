namespace HMS.Entity;

public class CreateCustomerEntity
{
    public int StoreId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public int AddressId { get; set; }
    public bool ActiveBool { get; set; }
    public int Active { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? LastUpdate { get; set; }
}

public class UpdateCustomerEntity
{
    public int CustomerId { get; set; }
    public int StoreId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public int AddressId { get; set; }
    public bool ActiveBool { get; set; }
    public int Active { get; set; }
    public DateTime? LastUpdate { get; set; }
}

public class GetCustomerEntity
{
    public int CustomerId { get; set; }
    public int StoreId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public int AddressId { get; set; }
    public bool ActiveBool { get; set; }
    public int Active { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdate { get; set; }
}