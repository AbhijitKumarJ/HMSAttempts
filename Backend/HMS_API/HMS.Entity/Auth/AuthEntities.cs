namespace HMS.Entity.Auth;

public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class TokenResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public int ExpiresIn { get; set; }
    public DateTime IssuedAt { get; set; }
}

public class RefreshTokenDto
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRevoked { get; set; }
}

public class CreateUserEntity
{
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public long? RoleId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }    
}

public class UpdateUserEntity
{
    public long UserId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public long? RoleId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class GetUserEntity
{
    public long UserId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public long? RoleId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// public class UpdateUserEntity
// {
//     public long UserId { get; set; }
//     public string Username { get; set; } = null!;
//     public string PasswordHash { get; set; } = null!;
//     public long? RoleId { get; set; }
//     public DateTime? CreatedAt { get; set; }
//     public DateTime? UpdatedAt { get; set; }
// }

// public class GetUserEntity
// {
//     public long UserId { get; set; }
//     public string Username { get; set; } = null!;
//     public string PasswordHash { get; set; } = null!;
//     public long? RoleId { get; set; }
//     public DateTime? CreatedAt { get; set; }
//     public DateTime? UpdatedAt { get; set; }
// }