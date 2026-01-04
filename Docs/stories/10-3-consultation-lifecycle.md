# Story 10.3: Consultation Lifecycle

**Status:** ready-for-dev

## Story

As a Doctor,
I want to explicitly "Start" and "End" a consultation,
So that we track the actual encounter duration and link it to the appointment.

## Acceptance Criteria

1.  **Start:** Clicking "Start Visit" on an Appointment creates a `clin_consultations` record.
2.  **Link:** The Consultation is linked to `sch_appointments`.
3.  **End:** Clicking "Finish" sets the `ended_at` timestamp.
4.  **State:** Active consultations appear on the "Occupied" dashboard.

## Technical Implementation

### Database
*   **Table:** `clin_consultations` (appointment_id, doctor_id, started_at, ended_at).

### Backend
*   **Logic:** Ensure only one active consultation per Doctor at a time (optional warning).
