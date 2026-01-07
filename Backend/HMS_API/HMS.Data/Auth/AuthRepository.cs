using HMS.Data.DBModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace HMS.Data.Auth;

public interface IAuthRepository
{
    object GetUserById(int id);
    User? GetUserByUsername(string username);
    User? GetUserWithRoles(string username);
    User? GetUserWithRolesById(int userId);
    Task<List<User>> GetDoctorsAsync(string? query);
    RefreshToken? CreateRefreshToken(RefreshToken token);
    RefreshToken? GetRefreshTokenByHash(string tokenHash);
    bool RevokeRefreshToken(Guid tokenId);
    bool RevokeAllUserRefreshTokens(int userId);
    void CleanupExpiredRefreshTokens();
}

public class AuthRepository : IAuthRepository
{
    private readonly HMSContext _context;

    public AuthRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user ?? new object();
    }

    public User? GetUserByUsername(string username)
    {
        return _context.Users.FirstOrDefault(u => u.Username == username);
    }

    public User? GetUserWithRoles(string username)
    {
        return _context.Users
            .Include(u => u.Roles)
            .FirstOrDefault(u => u.Username == username);
    }

    public User? GetUserWithRolesById(int userId)
    {
        return _context.Users
            .Include(u => u.Roles)
            .FirstOrDefault(u => u.Id == userId);
    }

    public async Task<List<User>> GetDoctorsAsync(string? query)
    {
        var doctorsQuery = _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Roles.Any(r => r.Name == "Doctor"));

        if (!string.IsNullOrWhiteSpace(query))
        {
            var searchTerm = $"%{query}%";
            doctorsQuery = doctorsQuery.Where(u => EF.Functions.ILike(u.Username, searchTerm));
        }

        return await doctorsQuery
            .OrderBy(u => u.Username)
            .ToListAsync();
    }

    public RefreshToken? CreateRefreshToken(RefreshToken token)
    {
        _context.RefreshTokens.Add(token);
        _context.SaveChanges();
        return token;
    }

    public RefreshToken? GetRefreshTokenByHash(string tokenHash)
    {
        return _context.RefreshTokens
            .Include(rt => rt.User)
            .ThenInclude(u => u.Roles)
            .FirstOrDefault(rt => rt.TokenHash == tokenHash);
    }

    public bool RevokeRefreshToken(Guid tokenId)
    {
        var token = _context.RefreshTokens.Find(tokenId);
        if (token == null) return false;

        token.IsRevoked = true;
        token.RevokedAt = DateTime.UtcNow;
        _context.SaveChanges();
        return true;
    }

    public bool RevokeAllUserRefreshTokens(int userId)
    {
        var tokens = _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !(rt.IsRevoked==true))
            .ToList();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }

        _context.SaveChanges();
        return true;
    }

    public void CleanupExpiredRefreshTokens()
    {
        var expiredTokens = _context.RefreshTokens
            .Where(rt => rt.ExpiresAt < DateTime.UtcNow)
            .ToList();

        _context.RefreshTokens.RemoveRange(expiredTokens);
        _context.SaveChanges();
    }
}