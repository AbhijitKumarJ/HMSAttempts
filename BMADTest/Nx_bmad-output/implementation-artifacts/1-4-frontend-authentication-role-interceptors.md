# Story 1.4: Frontend Authentication & Role Interceptors

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Frontend Developer,
I want to implement the Angular Auth Service and Interceptors,
so that every API request includes the JWT and handles 401 errors gracefully.

## Acceptance Criteria

1.  **Auth Service Implemented:** An `AuthService` (Signals-based) manages user state (`currentUser`, `isAuthenticated`, `activeRole`) and handles login/logout actions using `HttpClient`.
2.  **JWT Interceptor:** A functional `HttpInterceptorFn` automatically attaches the `Authorization: Bearer <token>` header to all outgoing requests to the API domain.
3.  **Error Interceptor:** A functional `HttpInterceptorFn` catches 401 Unauthorized errors.
4.  **Token Refresh Flow:** On 401, the interceptor attempts to refresh the token *once* using a `RefreshTokenService`.
    -   If refresh succeeds: Replays the original request with the new token.
    -   If refresh fails: Logs the user out and redirects to `/login`.
    -   Concurrency: Handles multiple concurrent 401s by queuing requests and refreshing only once (using `BehaviorSubject` semaphore pattern).
5.  **Role Guard:** An `authGuard` functional guard prevents navigation to protected routes if not authenticated.
6.  **Role Awareness:** The `AuthService` parses the JWT `roles` claim and exposes a `hasRole(role)` computed signal.

## Tasks / Subtasks

- [ ] 1. Generate Auth Service
    - [ ] Run `npx nx g @nx/angular:service auth --project=web-client`
    - [ ] Implement Signals state: `user = signal<User | null>(null)`, `token = signal<string | null>(null)`
    - [ ] Implement `login(credentials)`: Posts to `/api/auth/login`, sets signals, persists token to localStorage.
    - [ ] Implement `logout()`: Clears signals and localStorage, navigates to login.
- [ ] 2. Implement Functional Auth Interceptor
    - [ ] Create `apps/web-client/src/app/auth/auth.interceptor.ts`
    - [ ] Define `authInterceptor: HttpInterceptorFn`
    - [ ] Logic: Clone request, add `Authorization` header if token exists in `AuthService`.
- [ ] 3. Implement Functional Error/Refresh Interceptor
    - [ ] Create `apps/web-client/src/app/auth/error.interceptor.ts`
    - [ ] Logic: `catchError` -> check status 401 -> call `handle401Error`.
    - [ ] Implement `handle401Error`: Use `RefreshTokenService` (or method in `AuthService`) to refresh.
    - [ ] Implement queuing logic: If `isRefreshing` is true, wait for `refreshTokenSubject`.
- [ ] 4. Implement Auth Guard
    - [ ] Create `apps/web-client/src/app/auth/auth.guard.ts`
    - [ ] Logic: Check `authService.isAuthenticated()`. If false, redirect to `/login`.
- [ ] 5. Register Interceptors
    - [ ] Update `apps/web-client/src/app/app.config.ts`: Add `provideHttpClient(withInterceptors([authInterceptor, errorInterceptor]))`.
- [ ] 6. Unit Tests
    - [ ] Test `AuthService`: Login sets signals correctly.
    - [ ] Test `authInterceptor`: Headers are added.
    - [ ] Test `errorInterceptor`: 401 triggers refresh logic (mock backend).

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **Functional Interceptors:** Do NOT use class-based `HTTP_INTERCEPTORS`. Use `HttpInterceptorFn`.
    ```typescript
    export const authInterceptor: HttpInterceptorFn = (req, next) => {
      const authService = inject(AuthService);
      // ...
      return next(req);
    };
    ```
-   **Signals for State:** The `AuthService` MUST expose state via Signals (`computed` for derived state like `isAuthenticated`).
    ```typescript
    isAuthenticated = computed(() => !!this.token());
    ```
-   **Circular Dependency:** Be careful injecting `AuthService` into the interceptor if `AuthService` also uses `HttpClient`. Usually fine with functional interceptors, but if circular dep occurs, use `Injector` or separate the Token storage into a `TokenStorageService`.
-   **Security:**
    -   Persist JWT in `localStorage` (for MVP - HTTPOnly cookies preferred in Prod but requires backend change).
    -   Ensure `logout` handles full cleanup.

### Project Structure Requirements

-   Directory: `apps/web-client/src/app/auth/`
-   Files: `auth.service.ts`, `auth.interceptor.ts`, `error.interceptor.ts`, `auth.guard.ts`

### References

-   [Source: _bmad-output/planning-artifacts/architecture.md#frontend-architecture] (Angular Signals & Standalone)
-   [Source: Web Research] (Best practices for Angular 17+ functional interceptors)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
