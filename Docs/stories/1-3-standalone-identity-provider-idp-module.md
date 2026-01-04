# Story 1.3: Standalone Identity Provider (IdP) Module

**Status:** ready-for-dev

## Story

As a Security Architect,
I want to build the Auth module with JWT issuance,
So that users can authenticate and receive secure tokens.

## Acceptance Criteria

1.  **Login Endpoint:** `POST /api/auth/login` accepts username/password.
2.  **Token Issuance:** Returns a JWT signed with a secure secret key.
3.  **Claims:** JWT includes `sub` (UserId), `name` (Username), and `role` (Active Role).
4.  **Validation:** Invalid credentials return HTTP 401 Unauthorized.
5.  **Refresh Flow:** Use a **Secure, HTTP-Only Cookie** (SameSite=Strict) to store the `refreshToken`. The `accessToken` is returned in the response body.

## Technical Implementation

### Backend
*   **Controller:** `AuthController.cs`
    *   `POST /login`: Returns Access Token (JSON), Sets Refresh Token (Cookie).
    *   `POST /refresh`: Reads Cookie, Validates, Returns new Access Token.
    *   `POST /logout`: Clears the Refresh Token Cookie.
*   **Service:** `AuthService` handling password hashing (BCrypt) and Token Generation (`System.IdentityModel.Tokens.Jwt`).
*   **Config:** Store JWT Secret, Issuer, and Audience in `appsettings.json`.
*   **Security:** Ensure passwords are NEVER stored in plain text.

### Models
*   **LoginRequest:** `{ "username": "...", "password": "..." }`
*   **TokenResponse:** `{ "accessToken": "...", "expiresIn": 3600 }`

### Data
*   **Table:** `users` (id, username, password_hash, is_active).
*   **Table:** `user_roles` (user_id, role_name).
