# Story 5.3: Result Processing Worker

**Status:** ready-for-dev

## Story

As a System,
I want to process "Result.Available" events from the Lab,
So that the clinical dashboard is updated.

## Acceptance Criteria

1.  **Trigger:** Listens for `Result.Available` events in `app_events`.
2.  **Action:** Updates the central Patient Record (or Clinical Data Repository) with the results.
3.  **Notification:** (Optional for MVP) Triggers a signal/notification to the ordering provider.
4.  **Display:** The result becomes visible on the Patient Timeline.

## Technical Implementation

### Backend
*   **Worker:** `ClinicalResultWorker : BackgroundService`.
*   **Logic:**
    1.  Poll for `Result.Available`.
    2.  Insert into `clin_lab_results` (or update existing order status).
    3.  Mark event as `completed`.

### Frontend
*   **Refresh:** The `PatientDashboard` timeline polls or refreshes to show the new result.
