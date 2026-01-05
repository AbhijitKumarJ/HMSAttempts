namespace HMS.Entity.Auth;

public class DoctorDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
}
