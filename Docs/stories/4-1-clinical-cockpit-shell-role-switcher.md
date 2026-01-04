# Story 4.1: Clinical Cockpit Shell & Role Switcher

**Status:** ready-for-dev

## Story

As a User with multiple roles,
I want to switch between "Nurse" and "Receptionist" views,
So that I see only the tools relevant to my current task.

## Acceptance Criteria

1.  **Role Selector:** A dropdown in the top navigation bar showing the current active role.
2.  **State Management:** Switching roles updates the global application state (Signals/Store) without a hard page reload.
3.  **UI Adaptability:**
    *   **Receptionist Mode:** Shows Billing, Registration, Scheduling.
    *   **Nurse Mode:** Shows Triage, Clinical Cockpit, Vitals.
4.  **Security:** The frontend JWT (or refresh flow) is refreshed/updated to reflect the permissions of the newly selected role (if backend requires role-specific tokens). Or, the frontend filters visibility based on the active role claim.

## Technical Implementation

### Frontend (Angular)
*   **Service:** `AuthService` with a `currentUser` Signal that includes `activeRole`.
*   **Component:** `RoleSwitcherComponent` in the `ShellComponent` header.
*   **Logic:**
    *   `switchRole(newRole)` method updates the signal.
    *   `computed()` signals used in templates to toggle visibility (e.g., `showClinicalTools = computed(() => this.auth.activeRole() === 'Nurse')`).

### Backend (ASP.NET Core)
*   Ensure `ClaimsPrincipal` logic can handle "Active Role" if specific data filtering is needed on the server side.
