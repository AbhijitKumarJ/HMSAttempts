# Story 2.3: Patient Registration Frontend

**Status:** ready-for-dev

## Story

As a Receptionist,
I want a rapid registration form in the web client,
So that I can quickly register a patient without navigating complex menus.

## Acceptance Criteria

1.  **UI Component:** A "Quick Register" button on the Dashboard.
2.  **Modal:** Clicking the button opens an Angular Material Dialog.
3.  **Form:** Inputs for First Name, Last Name, Gender.
4.  **Submission:**
    *   Calls `POST /api/patients/emergency`.
    *   Shows a loading spinner.
    *   On success, closes modal and shows a "Patient Registered: [MRN]" toast.
5.  **Validation:** Basic required field validation.

## Technical Implementation

### Angular Components
*   **Dialog:** `EmergencyRegistrationDialogComponent` (Standalone).
*   **Service:** `PatientService.registerEmergency(data)`.
*   **State:** Use `MatDialog` service to open.

### UX
*   Focus first input field on open.
*   Allow `Enter` key to submit.
