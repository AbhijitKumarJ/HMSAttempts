# Story 2.2: Emergency Registration API & Logic

**Status:** ready-for-dev

## Story

As a Backend Developer,
I want to implement the Emergency Registration endpoint,
So that staff can create a patient record with minimal data.

## Acceptance Criteria

1.  **Endpoint:** `POST /api/patients/emergency`
2.  **Input:** `{ "firstName": "...", "lastName": "...", "gender": "..." }`
3.  **Logic:**
    *   Generate unique MRN (Format: `MRN-{Year}-{Sequence}`).
    *   Set `IsEmergency` flag to true.
    *   Save to database.
    *   Publish `Patient.Created` event to Message Bus.
4.  **Output:** Returns 201 Created with the generated MRN and Patient ID.

## Technical Implementation

### Controller
*   **Name:** `PatientController`
*   **Attribute:** `[ApiController]`, `[Route("api/[controller]")]`

### Services
*   **MRN Generator:** A service to generate collision-free MRNs (e.g., using a DB sequence or Redis counter).
*   **Event Publisher:** Inject `IEventBus` to publish the event.

### Validation
*   Ensure First/Last Name are not empty.
*   Gender must be valid (M/F/O/U).
