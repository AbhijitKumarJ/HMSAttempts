# Story 9.3: Audit Log Viewer

**Status:** ready-for-dev

## Story

As an Compliance Officer,
I want to view a history of critical system actions,
So that I can investigate unauthorized changes or errors.

## Acceptance Criteria

1.  **UI:** An "Audit Logs" page (Admin only).
2.  **Filter:** Filter by Date Range, User, Entity Type (e.g., "Invoice"), or specific Entity ID.
3.  **Data:** Shows Timestamp, User, Action, OldValue, NewValue.
4.  **Pagination:** Server-side pagination handles large datasets.

## Technical Implementation

### Backend
*   **Endpoint:** `GET /api/audit/logs`.
*   **Query:** Support robust filtering using `IQueryable` (e.g., using Sieve or custom filter model).

### Frontend
*   **Component:** `AuditLogGridComponent` using Angular Material Table with Sort/Page headers.
