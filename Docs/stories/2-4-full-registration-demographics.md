# Story 2.4: Full Registration & Demographics

**Status:** ready-for-dev

## Story

As a Receptionist,
I want to update an existing patient record with full demographics,
So that we have complete contact and insurance information.

## Acceptance Criteria

1.  **Endpoint:** `PATCH /api/patients/{mrn}`
2.  **Scope:** Update Address, Email, Phone, Insurance Provider, Policy Number.
3.  **Validation:**
    *   Email format.
    *   Phone number format.
4.  **Audit:** Logs the update action in the audit trail.

## Technical Implementation

### Backend
*   **Dto:** `UpdatePatientDto` with optional fields.
*   **Logic:** Retrieve patient, update fields, `SaveChanges`.

### Frontend
*   **View:** `PatientDetailComponent` with an "Edit Demographics" mode or tab.
*   **Form:** Reactive Form with validation logic.
