# Story 1.4: Frontend Authentication & Role Interceptors

**Status:** ready-for-dev

## Story

As a Frontend Developer,
I want to implement the Angular Auth Service and Interceptors,
So that every API request includes the JWT and handles 401 errors gracefully.

## Acceptance Criteria

1.  **Interceptor Logic:** All outgoing HTTP requests to the API domain automatically attach the `Authorization: Bearer <token>` header.
2.  **Error Handling:** If the API returns 401 Unauthorized, the user is redirected to `/login`.
3.  **Auth Guard:** Protected routes (e.g., `/dashboard`) cannot be accessed without a valid token.
4.  **Role Guard:** Admin-only routes cannot be accessed by non-admin users.

## Technical Implementation

### Angular artifacts
*   **Service:** `AuthService` (manages `currentUser` signal, login/logout methods, token storage in localStorage/sessionStorage).
*   **Interceptor:** `authInterceptor` (Functional Interceptor pattern in Angular 21).
*   **Guards:** `canActivate` functional guards (`authGuard`, `roleGuard`).

### Integration
*   Register the interceptor in `app.config.ts` using `provideHttpClient(withInterceptors([authInterceptor]))`.
