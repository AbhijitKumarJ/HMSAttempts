# Story 3.4: Vital Signs Alerting Logic

**Status:** ready-for-dev

## Story

As a Nurse,
I want immediate visual feedback on abnormal vitals,
So that I can identify critical patients instantly.

## Acceptance Criteria

1.  **Real-time Validation:** As user types (debounce ~300ms), check against threshold rules.
2.  **Visuals:**
    *   High/Low Warning: Amber border.
    *   Critical: Red border + Warning Icon.
3.  **Summary:** If any critical values exist, flag the "Chief Complaint" or "Triage Summary" as High Priority.

## Technical Implementation

### Logic
*   **Service:** `VitalsValidationService` (Frontend).
*   **Rules:** Defined in constants (e.g., `HR > 120 = Critical`).

### UI
*   Use Angular Signals to compute `alertLevel` for each field.
*   Bind class `border-red-500` if `alertLevel() === 'Critical'`.
