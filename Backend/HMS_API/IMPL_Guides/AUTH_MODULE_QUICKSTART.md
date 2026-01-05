# Auth Module Quick Start Guide

## Step 1: Setup Database

Ensure `refresh_tokens` table exists:

```sql
-- Create refresh_tokens table
CREATE TABLE refresh_tokens (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id INT REFERENCES users(id) ON DELETE CASCADE,
    token_hash VARCHAR(255) UNIQUE NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    revoked_at TIMESTAMP NULL,
    is_revoked BOOLEAN DEFAULT FALSE
);
CREATE INDEX idx_refresh_tokens_user_id ON refresh_tokens(user_id);
CREATE INDEX idx_refresh_tokens_token_hash ON refresh_tokens(token_hash);
```

## Step 2: Configure JWT

Ensure `appsettings.json` has JWT configuration:

```json
{
  "Jwt": {
    "Secret": "YOUR_SECURE_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "HMS",
    "Audience": "HMS.Clients",
    "AccessTokenExpirationMinutes": 60
  }
}
```

## Step 3: Build and Run

```bash
cd Backend/HMS_API
dotnet build
cd HMS.API
dotnet run
```

## Step 4: Test Authentication

### Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{
    "username": "testuser",
    "password": "password123"
  }'
```

Expected response:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "issuedAt": "2025-01-05T00:00:00Z"
}
```

Refresh token is set in `cookies.txt`:
```
Set-Cookie: refreshToken=token-value; HttpOnly; SameSite=Strict
```

### Access Protected Endpoint
```bash
ACCESS_TOKEN="your-jwt-token-here"

curl -X GET http://localhost:5000/api/clinical/consultations \
  -H "Authorization: Bearer $ACCESS_TOKEN"
```

### Refresh Token
```bash
curl -X POST http://localhost:5000/api/auth/refresh \
  -b cookies.txt \
  -c cookies.txt
```

### Logout
```bash
curl -X POST http://localhost:5000/api/auth/logout \
  -b cookies.txt
```

## Step 5: Seed Test Users

Create a test user with hashed password:

```sql
-- This SQL requires BCrypt to be installed
-- Generate hash using online tool or C# code
INSERT INTO users (username, password_hash, is_active) VALUES
('doctor1', '$2a$11$abcdefghijklmnopqrstuvw', TRUE);

-- Assign role
INSERT INTO user_roles (user_id, role_id)
VALUES ((SELECT id FROM users WHERE username = 'doctor1'), 1);
```

**Generate BCrypt Hash:**

Using C#:
```csharp
using BCrypt.Net;
var hash = BCrypt.HashPassword("password123");
Console.WriteLine(hash);
```

Using online tool: https://bcrypt-generator.com/

## Step 6: Protect Endpoints

Add `[Authorize]` attribute to protected endpoints:

```csharp
[HttpGet("patients")]
[Authorize]  // Requires valid JWT
public async Task<IActionResult> GetPatients()
{
    // Only authenticated users can access
}
```

## Step 7: Run Tests

```bash
cd Backend/HMS_API

# Run all auth tests
dotnet test HMS.Tests/HMS.Tests.csproj --filter "FullyQualifiedName~AuthTests"

# Run specific test class
dotnet test HMS.Tests/HMS.Tests.csproj --filter "FullyQualifiedName~AuthServiceTests"

# Run specific test
dotnet test HMS.Tests/HMS.Tests.csproj --filter "FullyQualifiedName~LoginAsync_ValidCredentials_ReturnsToken"

# Verbose output
dotnet test HMS.Tests/HMS.Tests.csproj --filter "FullyQualifiedName~AuthTests" --logger "console;verbosity=detailed"
```

## Common Issues

### 1. Login returns 401

**Cause:** Invalid credentials or inactive user

**Solution:**
```bash
# Check user exists and is active
psql -U postgres -d hms -c "SELECT id, username, is_active FROM users WHERE username = 'testuser';"

# Check password hash (for comparison only, never log plain passwords)
psql -U postgres -d hms -c "SELECT password_hash FROM users WHERE username = 'testuser';"
```

### 2. Refresh token invalid

**Cause:** Token expired, revoked, or not found

**Solution:**
```bash
# Check refresh token in database
psql -U postgres -d hms -c "SELECT * FROM refresh_tokens ORDER BY created_at DESC LIMIT 5;"

# Check if token is expired or revoked
psql -U postgres -d hms -c "SELECT is_revoked, expires_at, NOW() FROM refresh_tokens WHERE id = 'uuid-here';"
```

### 3. JWT validation fails

**Cause:** Wrong secret or expired token

**Solution:**
```bash
# Check JWT secret in appsettings.json
cat HMS.API/appsettings.json | grep -A 5 "Jwt"

# Verify secret matches across environments
echo $JWT_SECRET
```

### 4. Tests fail to build

**Cause:** Missing packages or configuration

**Solution:**
```bash
# Restore packages
dotnet restore HMS.Business/HMS.Business.csproj
dotnet restore HMS.Tests/HMS.Tests.csproj

# Verify Moq is installed
dotnet list package --include-transitive | grep Moq
```

## Authentication Flow Diagram

```
┌─────────┐
│ Browser  │
└────┬────┘
     │
     │ 1. POST /api/auth/login
     │    { username, password }
     │
     ↓
┌─────────────────────────┐
│   Backend Server      │
│  ┌──────────────────┐  │
│  │ AuthService       │  │
│  │ Validate Credentials│  │
│  └──────────────────┘  │
│     ↓                 │
│  ┌──────────────────┐  │
│  │ Generate JWT      │  │  │ claims: sub, name, role
│  │ (1 hour)         │  │
│  └──────────────────┘  │
│     ↓                 │
│  ┌──────────────────┐  │
│  │ Generate Refresh   │  │  │ 7 days, HttpOnly cookie
│  └──────────────────┘  │
│     ↓                 │
│  ┌──────────────────┐  │
│  │ Store in DB      │  │  │ refresh_tokens table
│  └──────────────────┘  │
└────┬───────────────────┘
     │
     │ 2. Response with JWT
     │    + Set-Cookie: refreshToken
     │
     ↓
┌─────────┐
│ Browser  │
│ (stores   │
│  cookie)  │
└────┬────┘
     │
     │ 3. API calls with Bearer token
     │    Authorization: Bearer eyJhbGc...
     │
     ↓
┌─────────────────────────┐
│   Backend Server      │
│  ┌──────────────────┐  │
│  │ Validate JWT      │  │
│  └──────────────────┘  │
│     ↓                 │
│  ┌──────────────────┐  │
│  │ Return Data      │  │
│  └──────────────────┘  │
└────┬───────────────────┘
     │
     │ 4. Token expires (1 hour)
     │
     ↓
┌─────────┐
│ Browser  │
└────┬────┘
     │
     │ 5. POST /api/auth/refresh
     │    Cookie: refreshToken=...
     │
     ↓
┌─────────────────────────┐
│   Backend Server      │
│  ┌──────────────────┐  │
│  │ Validate Token    │  │
│  └──────────────────┘  │
│     ↓                 │
│  ┌──────────────────┐  │
│  │ Generate New JWT  │  │
│  └──────────────────┘  │
│     ↓                 │
│  ┌──────────────────┐  │
│  │ Generate New       │  │
│  │ Refresh Token    │  │  │ Rotate token
│  └──────────────────┘  │
│     ↓                 │
│  ┌──────────────────┐  │
│  │ Revoke Old       │  │
│  │ Refresh Token    │  │
│  └──────────────────┘  │
└────┬───────────────────┘
     │
     │ 6. Response with new JWT
     │    + Set new cookie
     │
     ↓
┌─────────┐
│ Browser  │
│ (update   │
│  cookie)  │
└─────────┘
```

## Role-Based Authorization

### Authorize by Role

```csharp
// Require specific role
[HttpGet("patients")]
[Authorize(Roles = "Doctor")]
public async Task<IActionResult> GetPatients()
{
    // Only doctors can access
}
```

### Multiple Roles

```csharp
// Require any of multiple roles
[HttpGet("patients")]
[Authorize(Roles = "Doctor,Nurse")]
public async Task<IActionResult> GetPatients()
{
    // Doctors and nurses can access
}
```

### Custom Policy

```csharp
// In Program.cs
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanPrescribe", policy =>
        policy.RequireRole("Doctor", "Nurse"));
});

// In Controller
[HttpGet("prescriptions")]
[Authorize(Policy = "CanPrescribe")]
public async Task<IActionResult> GetPrescriptions()
{
    // Users with Doctor or Nurse role
}
```

## Security Checklist

Before deploying to production, ensure:

- ✅ JWT secret is at least 32 characters
- ✅ JWT secret is stored in environment variable or secret manager
- ✅ HTTPS is enabled (set `Secure = true` for cookies)
- ✅ Database credentials are secured
- ✅ Password complexity requirements are enforced
- ✅ Rate limiting is configured on login endpoint
- ✅ Failed login attempts are logged
- ✅ Account lockout is implemented
- ✅ Refresh token expiration is appropriate (7 days recommended)
- ✅ Access token expiration is appropriate (1 hour recommended)

## Troubleshooting Commands

### Check Database Connection
```bash
psql -U postgres -h localhost -d hms -c "SELECT NOW();"
```

### Check Users Table
```bash
psql -U postgres -d hms -c "SELECT id, username, is_active FROM users LIMIT 10;"
```

### Check Refresh Tokens
```bash
psql -U postgres -d hms -c "SELECT id, user_id, is_revoked, expires_at FROM refresh_tokens ORDER BY created_at DESC LIMIT 10;"
```

### Clean Expired Tokens
```bash
# Manual cleanup (also done automatically in future)
psql -U postgres -d hms -c "DELETE FROM refresh_tokens WHERE expires_at < NOW();"
```

### Test JWT Decoding
Use https://jwt.io/ to decode and verify JWT structure:
```json
{
  "sub": "1",
  "name": "doctor1",
  "role": "Doctor",
  "jti": "guid-here",
  "exp": 1736064000,
  "iat": 1736060400
}
```

## Next Steps

After authentication is working:

1. **Implement Role Switching** - Allow users to change active role without re-login
2. **Add Protected Routes** - Apply `[Authorize]` to all sensitive endpoints
3. **Implement Permission System** - More granular access control
4. **Add Audit Logging** - Log all authentication events
5. **Implement 2FA** - Two-factor authentication for critical operations

## Resources

- Full Implementation: `AUTH_MODULE_IMPLEMENTATION.md`
- Story Details: `Docs/stories/1-3-standalone-identity-provider-idp-module.md`
- Tests: `HMS.Tests/Tests/Auth/AuthServiceTests.cs`
- Tests: `HMS.Tests/Tests/Auth/AuthControllerTests.cs`

## Support

If you encounter issues:

1. Check the logs in the console output
2. Verify database schema is correct
3. Ensure JWT configuration is set
4. Run tests to verify functionality
5. Check database connection string

The auth module is now ready for integration with other HMS components!
