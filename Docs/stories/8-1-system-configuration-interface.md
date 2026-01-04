# Story 8.1: System Configuration Interface

**Status:** ready-for-dev

## Story

As an Admin,
I want a UI to manage global system settings,
So that I can configure the clinic's name and defaults.

## Acceptance Criteria

1.  **UI:** A "System Settings" page accessible only to Administrators.
2.  **Fields:** Clinic Name, Default Currency (USD/EUR/INR), Default Timezone.
3.  **Persistence:** Settings are saved to a `sys_config` table (Key-Value store).
4.  **Application:** The Frontend displays the configured Clinic Name in the header.

## Technical Implementation

### Database
*   **Table:** `sys_config`
    *   `key` (VARCHAR, PK)
    *   `value` (VARCHAR)
    *   `description` (VARCHAR)

### Backend
*   **Endpoint:** `GET /api/system/config` (Public/Cached).
*   **Endpoint:** `PUT /api/system/config` (Admin Only).
*   **Cache:** Use `IMemoryCache` to avoid hitting DB on every request.

### Frontend
*   **Service:** `ConfigService` loads settings on `APP_INITIALIZER`.
*   **Component:** `AdminSettingsComponent`.
