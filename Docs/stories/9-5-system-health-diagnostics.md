# Story 9.5: System Health & Diagnostics

**Status:** ready-for-dev

## Story

As a DevOps Engineer,
I want an automated health check endpoint,
So that I know if the database or queue infrastructure is down.

## Acceptance Criteria

1.  **Endpoint:** `/health` returns 200 OK + JSON summary.
2.  **Checks:**
    *   PostgreSQL connectivity.
    *   Disk Space (host).
    *   Memory usage.
3.  **UI:** A simple `/health-ui` (optional) or Status Indicator in the Admin footer.

## Technical Implementation

### Backend
*   **Library:** `Microsoft.AspNetCore.Diagnostics.HealthChecks`.
*   **Library:** `AspNetCore.HealthChecks.Npgsql`.
*   **Configuration:** Register checks in `Program.cs`.
