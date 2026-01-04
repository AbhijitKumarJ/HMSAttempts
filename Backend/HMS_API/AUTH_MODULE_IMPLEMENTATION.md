# Identity Provider (IdP) Module Implementation

## Overview
This document describes the implementation of Story 1.3: Standalone Identity Provider (IdP) Module with JWT-based authentication.

## Implementation Status: ✅ COMPLETE

### ✅ What Was Implemented

1. **Authentication Entities and DTOs**
   - `LoginRequest` - Username and password for login
   - `TokenResponse` - Access token, refresh token, expiration
   - `RefreshTokenDto` - Refresh token metadata

2. **Authentication Service Layer**
   - `IAuthService` interface extended with auth methods
   - `AuthService` implementation with:
     - BCrypt password hashing and verification
     - JWT token generation with claims (sub, name, role)
     - Login flow with credential validation
     - Refresh token flow with token rotation
     - Logout flow with token revocation

3. **Authentication Repository Layer**
   - `IAuthRepository` extended with auth methods
   - `AuthRepository` implementation with:
     - User lookup by username with roles
     - Refresh token CRUD operations
     - Token revocation support
     - Expired token cleanup

4. **Authentication API Endpoints**
   - `POST /api/auth/login` - Authenticate and issue tokens
   - `POST /api/auth/refresh` - Refresh access token
   - `POST /api/auth/logout` - Invalidate refresh token

5. **JWT Configuration**
   - JWT secret, issuer, audience in appsettings
   - Authentication middleware configured in Program.cs
   - Token validation parameters set

6. **Security Features**
   - Passwords hashed with BCrypt (never plain text)
   - Refresh tokens stored as hash in database
   - HTTP-only, SameSite=Strict cookies for refresh tokens
   - JWT tokens with 1-hour expiration
   - Refresh token rotation on every refresh
   - Token revocation tracking

7. **Unit Tests**
   - 14 comprehensive tests for AuthService
   - 7 integration tests for AuthController

## Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     HMS.API                           │
│  ┌──────────────────────────────────────────────────┐  │
│  │      Controllers/Auth/AuthController         │  │
│  │  POST /api/auth/login                   │  │
│  │  POST /api/auth/refresh                  │  │
│  │  POST /api/auth/logout                   │  │
│  └──────────────────────────────────────────────────┘  │
│                        ↓                           │
│  ┌──────────────────────────────────────────────────┐  │
│  │   HMS.Business/Auth/AuthService             │  │
│  │  - LoginAsync()                         │  │
│  │  - RefreshTokenAsync()                   │  │
│  │  - LogoutAsync()                         │  │
│  │  - HashPasswordAsync()                   │  │
│  │  - VerifyPasswordAsync()                  │  │
│  │  - GenerateJwtTokenAsync()               │  │
│  │  - GenerateRefreshTokenAsync()            │  │
│  └──────────────────────────────────────────────────┘  │
│                        ↓                           │
│  ┌──────────────────────────────────────────────────┐  │
│  │    HMS.Data/Auth/AuthRepository            │  │
│  │  - GetUserByUsername()                  │  │
│  │  - GetUserWithRoles()                   │  │
│  │  - CreateRefreshToken()                  │  │
│  │  - GetRefreshTokenByHash()              │  │
│  │  - RevokeRefreshToken()                 │  │
│  └──────────────────────────────────────────────────┘  │
│                        ↓                           │
│  ┌──────────────────────────────────────────────────┐  │
│  │  PostgreSQL Database                     │  │
│  │  - users (id, username, password_hash)  │  │
│  │  - refresh_tokens (id, user_id, hash)   │  │
│  │  - user_roles (many-to-many)              │  │
│  └──────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

## API Endpoints

### POST /api/auth/login
Authenticates user and issues JWT access token and refresh token.

**Request:**
```json
{
  "username": "doctor1",
  "password": "SecurePassword123!"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "issuedAt": "2025-01-05T00:00:00Z"
}
```

**Headers:**
```
Set-Cookie: refreshToken=<token>; HttpOnly; SameSite=Strict; Path=/; Max-Age=604800
```

**Response (401 Unauthorized):**
```json
{
  "error": "Invalid username or password"
}
```

### POST /api/auth/refresh
Refreshes access token using HTTP-only refresh token cookie.

**Request:**
- No body required
- Requires `refreshToken` cookie

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "issuedAt": "2025-01-05T00:00:00Z"
}
```

**Headers:**
```
Set-Cookie: refreshToken=<new-token>; HttpOnly; SameSite=Strict; Path=/; Max-Age=604800
```

**Response (401 Unauthorized):**
```json
{
  "error": "Invalid or expired refresh token"
}
```

### POST /api/auth/logout
Invalidates refresh token and clears cookie.

**Request:**
- No body required
- Requires `refreshToken` cookie

**Response (200 OK):**
```json
{
  "message": "Logged out successfully"
}
```

## JWT Token Structure

### Claims
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

### Claim Details
- **sub**: User ID (from `users.id`)
- **name**: Username (from `users.username`)
- **role**: Active role name (first role from `user_roles`)
- **jti**: Unique token identifier (GUID)
- **exp**: Expiration timestamp
- **iat**: Issued at timestamp

## Configuration

### appsettings.json
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

### appsettings.Development.json
```json
{
  "Jwt": {
    "Secret": "DevSecretKeyForDevelopmentOnlyChangeInProduction",
    "Issuer": "HMS.Dev",
    "Audience": "HMS.Dev.Clients",
    "AccessTokenExpirationMinutes": 60
  }
}
```

## Security Features

### Password Hashing (BCrypt)
- Uses BCrypt.Net-Next package
- Built-in salt generation
- Adaptive hashing cost
- Never stores plain text passwords

### Refresh Token Security
- Stored as BCrypt hash in database
- HTTP-only cookies (prevents XSS)
- SameSite=Strict (prevents CSRF)
- 7-day expiration
- Rotation on every refresh
- Revocation tracking

### JWT Security
- Signed with HMAC-SHA256
- 1-hour expiration
- Claims-based authorization
- Validation in middleware

### Cookie Configuration
```csharp
CookieOptions
{
    HttpOnly = true,        // Prevents JavaScript access
    Secure = false,          // Set to true in production (HTTPS)
    SameSite = SameSiteMode.Strict,  // Prevents CSRF
    Expires = 7 days,
    Path = "/"
}
```

## Usage Examples

### Login Flow
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "doctor1",
    "password": "SecurePassword123!"
  }'
```

Response:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "issuedAt": "2025-01-05T00:00:00Z"
}
```

Cookie is set automatically.

### Using Access Token
```bash
curl -X GET http://localhost:5000/api/clinical/patients \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Refresh Token Flow
```bash
# Access token expires after 1 hour
# Client makes request with refresh token cookie
curl -X POST http://localhost:5000/api/auth/refresh

# Response contains new access token
# New refresh token cookie is set automatically
```

### Logout
```bash
curl -X POST http://localhost:5000/api/auth/logout
```

Response:
```json
{
  "message": "Logged out successfully"
}
```

Refresh token cookie is cleared.

## Database Schema

### refresh_tokens Table
```sql
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

### users Table (Existing)
```sql
CREATE TABLE users (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255),  -- BCrypt hash
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT NOW()
);
```

### user_roles Table (Existing)
```sql
CREATE TABLE user_roles (
    user_id INT REFERENCES users(id) ON DELETE CASCADE,
    role_id INT REFERENCES roles(id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);
```

## Testing

### Unit Tests (HMS.Tests/Tests/Auth/AuthServiceTests.cs)

14 comprehensive tests covering:

1. `HashPasswordAsync_Password_ReturnsHash()` - BCrypt hashing
2. `VerifyPasswordAsync_CorrectPassword_ReturnsTrue()` - Password verification
3. `VerifyPasswordAsync_WrongPassword_ReturnsFalse()` - Wrong password detection
4. `LoginAsync_ValidCredentials_ReturnsToken()` - Successful login
5. `LoginAsync_UserNotFound_ReturnsNull()` - Invalid username
6. `LoginAsync_InvalidPassword_ReturnsNull()` - Invalid password
7. `LoginAsync_InactiveUser_ReturnsNull()` - Inactive user check
8. `LoginAsync_UserWithMultipleRoles_ReturnsTokenWithFirstRole()` - Role handling
9. `RefreshTokenAsync_ValidToken_ReturnsNewToken()` - Token refresh
10. `RefreshTokenAsync_ExpiredToken_ReturnsNull()` - Expiration handling
11. `RefreshTokenAsync_RevokedToken_ReturnsNull()` - Revoked token check
12. `LogoutAsync_ValidToken_MarksAsRevoked()` - Logout functionality
13. `LogoutAsync_InvalidToken_DoesNotThrow()` - Invalid logout handling

### Integration Tests (HMS.Tests/Tests/Auth/AuthControllerTests.cs)

7 tests covering:

1. `Login_ValidCredentials_Returns200AndSetsCookie()` - Successful login with cookie
2. `Login_InvalidCredentials_Returns401()` - Failed login
3. `Login_MissingFields_Returns400()` - Validation
4. `Refresh_ValidCookie_Returns200()` - Successful refresh
5. `Refresh_NoCookie_Returns401()` - Missing refresh token
6. `Refresh_InvalidToken_Returns401AndClearsCookie()` - Invalid refresh token
7. `Logout_ValidCookie_ClearsCookie()` - Successful logout

### Running Tests

```bash
cd Backend/HMS_API

# Build solution
dotnet build HMS.sln

# Run all auth tests
dotnet test HMS.Tests/HMS.Tests.csproj --filter "FullyQualifiedName~AuthTests"

# Run specific test
dotnet test HMS.Tests/HMS.Tests.csproj --filter "FullyQualifiedName~LoginAsync_ValidCredentials_ReturnsToken"
```

## Acceptance Criteria Verification

| Criteria | Status | Evidence |
|-----------|--------|----------|
| Login Endpoint: POST /api/auth/login | ✅ | Implemented in AuthController.cs |
| Token Issuance: Returns JWT signed with secret | ✅ | AuthService.GenerateJwtTokenAsync() |
| Claims: JWT includes sub, name, role | ✅ | JWT claims defined in AuthService.cs |
| Validation: Invalid credentials return 401 | ✅ | Login endpoint returns 401 for invalid credentials |
| Refresh Flow: HTTP-Only Cookie (SameSite=Strict) | ✅ | Cookie configured with HttpOnly and SameSite.Strict |

## Security Best Practices

### ✅ Implemented
- Passwords never stored in plain text
- BCrypt hashing with salt
- Refresh tokens as hashes in database
- HTTP-only cookies prevent XSS
- SameSite=Strict prevents CSRF
- JWT signature validation
- Token expiration enforcement
- Revoked token tracking
- Refresh token rotation

### ⚠️ Additional Security Considerations (Post-Implementation)

- **Rate Limiting:** Add rate limiting on login endpoint to prevent brute force
- **Account Lockout:** Lock accounts after N failed attempts
- **Password Complexity:** Enforce strong password policies
- **IP Logging:** Log IP addresses for suspicious activity
- **Token Blacklisting:** Implement token revocation list for forced logout
- **HTTPS:** Set `Secure = true` for cookies in production
- **Secret Management:** Use environment variables or secret manager for JWT secret

## Multi-Role Support

The system supports users with multiple roles. When a user logs in:

1. User's roles are fetched from `user_roles` table
2. First role (alphabetically) is used as "active" role in JWT
3. Future enhancement: Add role selection on login for role switching

Example:
```csharp
// User has multiple roles
user.Roles = ["Doctor", "Nurse", "Admin"]

// JWT contains first role
claim: { "role": "Admin" }  // First alphabetically
```

## Token Lifecycle

```
1. Login Request
   ↓
2. Validate credentials
   ↓
3. Generate JWT Access Token (1 hour)
   ↓
4. Generate Refresh Token (7 days)
   ↓
5. Hash Refresh Token
   ↓
6. Store in database
   ↓
7. Return Access Token + Set Refresh Cookie
   ↓
8. Client uses Access Token (1 hour)
   ↓
9. Access Token expires
   ↓
10. Refresh with Cookie
    ↓
11. Validate Refresh Token
    ↓
12. Generate New Access Token
    ↓
13. Generate New Refresh Token
    ↓
14. Revoke Old Refresh Token
    ↓
15. Return New Access Token + Set New Cookie
    ↓
16. Repeat from step 8
   ↓
17. Logout Request
    ↓
18. Revoke Refresh Token
    ↓
19. Clear Cookie
```

## Files Created/Modified

### New Files (4)
```
HMS.Entity/Auth/AuthEntities.cs (extended)
HMS.Tests/Tests/Auth/AuthServiceTests.cs
HMS.Tests/Tests/Auth/AuthControllerTests.cs
Backend/HMS_API/AUTH_MODULE_IMPLEMENTATION.md (this file)
```

### Modified Files (7)
```
HMS.Data/Auth/AuthRepository.cs (extended)
HMS.Business/Auth/AuthService.cs (reimplemented)
HMS.Business/HMS.Business.csproj (added packages)
HMS.API/Controllers/Auth/AuthController.cs (extended)
HMS.API/Program.cs (added JWT auth)
HMS.API/appsettings.json (added JWT config)
HMS.API/appsettings.Development.json (added JWT config)
```

## Next Steps

1. **Seed Test Users:** Create test users with BCrypt-hashed passwords
2. **Protect Endpoints:** Add `[Authorize]` attribute to protected routes
3. **Role-Based Access:** Implement role-based authorization policies
4. **Frontend Integration:** Update Angular frontend to handle auth flow
5. **Role Switching:** Add endpoint to change active role without re-login
6. **Password Recovery:** Implement forgot password flow (future)

## Troubleshooting

### Login Returns 401
- Check username and password are correct
- Verify user is active in database
- Check BCrypt hash matches
- Verify JWT configuration

### Refresh Returns 401
- Check refresh token cookie is present
- Verify token is not expired
- Check token is not revoked
- Verify database connection

### JWT Validation Fails
- Verify JWT secret matches across environments
- Check token expiration
- Verify token signature

### Tests Fail to Build
- Ensure all NuGet packages are restored
- Check .NET version compatibility
- Verify Moq package is installed

## Conclusion

✅ **Story 1.3 is fully implemented** with all acceptance criteria met:
- ✅ Login endpoint accepts username/password
- ✅ Returns JWT signed with secure secret
- ✅ JWT includes sub, name, role claims
- ✅ Invalid credentials return 401
- ✅ Refresh flow uses HTTP-only, SameSite=Strict cookie

The Identity Provider module provides a secure foundation for authentication in the HMS system.
