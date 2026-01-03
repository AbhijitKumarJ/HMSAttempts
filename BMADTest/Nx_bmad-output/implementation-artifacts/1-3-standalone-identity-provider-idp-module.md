# Story 1.3: Standalone Identity Provider (IdP) Module

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Security Architect,
I want to build the Auth module with JWT issuance,
so that users can authenticate and receive secure tokens.

## Acceptance Criteria

1.  **Auth Module Initialized:** A new directory `apps/api-server/src/auth` is created with standard FastAPI structure (`router.py`, `schemas.py`, `models.py`, `service.py`, `utils.py`).
2.  **User Model:** A `User` SQLAlchemy 2.0 model is defined with `id`, `username` (unique), `password_hash`, `roles` (JSONB list of strings), and `is_active`.
3.  **Password Security:** Passwords are hashed using `passlib[bcrypt]`.
4.  **JWT Issuance:** `POST /auth/login` accepts `username` and `password`, validates credentials, and returns a signed JWT (access_token) using `PyJWT` (HS256).
5.  **Token Payload:** The JWT payload includes `sub` (username) and `roles` (list of roles).
6.  **Configuration:** `SECRET_KEY`, `ALGORITHM`, and `ACCESS_TOKEN_EXPIRE_MINUTES` are loaded from environment variables (using Pydantic `BaseSettings`).
7.  **Standards Compliance:** API request/response models use Pydantic v2 with `camelCase` aliases (e.g., `accessToken` in JSON).
8.  **Seed Data:** A utility function exists to create an initial "admin" user if the users table is empty.

## Tasks / Subtasks

- [ ] 1. Setup Auth Dependencies
    - [ ] Add `pyjwt`, `passlib[bcrypt]` to `apps/api-server/pyproject.toml`
    - [ ] Add `python-multipart` (for OAuth2 form data if needed, though JSON preferred for custom IdP - stick to JSON `LoginRequest` for this specific story as per FR4)
- [ ] 2. Define User Database Model
    - [ ] Create `apps/api-server/src/auth/models.py`
    - [ ] Define `User` class inheriting from Base
    - [ ] Use `JSONB` for `roles` column
- [ ] 3. Implement Security Utilities
    - [ ] Create `apps/api-server/src/auth/utils.py`
    - [ ] Implement `PwdContext` with bcrypt
    - [ ] Implement `create_access_token` using `jwt.encode`
- [ ] 4. Define Pydantic Schemas
    - [ ] Create `apps/api-server/src/auth/schemas.py`
    - [ ] `LoginRequest`: username, password
    - [ ] `Token`: access_token, token_type
    - [ ] `TokenData`: username, roles
    - [ ] **Crucial:** Apply `model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)`
- [ ] 5. Implement Auth Service Logic
    - [ ] Create `apps/api-server/src/auth/service.py`
    - [ ] `authenticate_user(session, username, password)` -> User | None
    - [ ] `get_user(session, username)` -> User | None
- [ ] 6. Create API Router
    - [ ] Create `apps/api-server/src/auth/router.py`
    - [ ] Endpoint: `POST /login` (or `/auth/login` when mounted)
    - [ ] Return 401 for invalid credentials
- [ ] 7. Main Integration & Seeding
    - [ ] Mount auth router in `apps/api-server/src/main.py`
    - [ ] Add startup event to check/create default admin user (`admin` / `admin123` - change in prod!)
- [ ] 8. Unit Testing
    - [ ] Test password hashing (verify hash != password)
    - [ ] Test JWT generation (decode and verify claims)
    - [ ] Test Login endpoint (Success vs Failure)

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **JWT Library:** Use **PyJWT**, NOT `python-jose` (maintenance issues).
    ```python
    import jwt
    # ...
    encoded_jwt = jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)
    ```
-   **Pydantic V2:**
    ```python
    from pydantic import BaseModel, ConfigDict
    from pydantic.alias_generators import to_camel

    class Token(BaseModel):
        access_token: str
        token_type: str
        model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    ```
-   **Security:**
    -   Do NOT store plain text passwords.
    -   Do NOT commit `.env` files.
    -   Ensure `roles` are strictly typed as a list of strings in the Pydantic model.

### Project Structure Requirements

-   Directory: `apps/api-server/src/auth/`
-   Files: `models.py`, `schemas.py`, `service.py`, `router.py`, `utils.py`

### References

-   [Source: _bmad-output/planning-artifacts/architecture.md#authentication--security] (Auth Strategy)
-   [Source: _bmad-output/planning-artifacts/epics.md#story-13-standalone-identity-provider-idp-module] (Acceptance Criteria)
-   [Source: Web Research] (PyJWT recommendation over python-jose)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
