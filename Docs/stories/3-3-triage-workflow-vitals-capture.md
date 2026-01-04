# Story 3.3: Triage Workflow & Vitals Capture

**Status:** ready-for-dev

## Story

As a Nurse,
I want to record patient vitals during triage,
So that the doctor has baseline data.

## Acceptance Criteria

1.  **UI:** Triage Form displays inputs for standard vitals.
2.  **Storage:** Vitals are saved to the specialized `clin_vitals` table, NOT just the generic assessment table.
3.  **Fields:** BP (Systolic/Diastolic), Heart Rate, Temp, SpO2.
4.  **Workflow:** Can be part of a larger Triage Assessment or a standalone "Quick Vitals" entry.

## Technical Implementation

### Backend
*   **Entity:** `VitalSign` mapping to table `clin_vitals`.
*   **Endpoint:** `POST /api/clinical/vitals`.
*   **Logic:**
    *   Validate ranges (e.g., HR 0-300).
    *   Link to `pat_patients` (mandatory).
    *   Link to `clin_consultations` (optional).

### Frontend
*   **Component:** `VitalsCaptureComponent`.
*   **Form:** Reactive Form with real-time validation (Story 3.4 signals).
