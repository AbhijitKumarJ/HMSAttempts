# Story 2.2: Emergency Registration API & Logic

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Backend Developer,
I want to implement the Emergency Registration endpoint,
so that staff can create a patient record with minimal data.

## Acceptance Criteria

1.  **Endpoint Created:** `POST /patients/emergency` endpoint exists.
2.  **Minimal Input:** Accepts JSON payload with `firstName` (string), `lastName` (string), and `gender` (enum: 'M', 'F', 'O', 'U').
3.  **MRN Generation:** Automatically generates a unique 10-character alphanumeric MRN (e.g., "MRN-2024-0001" format per logic, or pure random if preferred by architecture - architecture specifies unique MRN, implies generator logic). *Refinement: Let's use `MRN-{YYYY}-{RANDOM_6_DIGITS}` to ensure uniqueness and readability.*
4.  **Database Persistence:** Creates a new `Patient` record in `pat_patients` table with:
    -   Generated MRN
    -   Provided Name/Gender
    -   `is_emergency = True` (or similar status flag if added to schema, otherwise implicit by missing data) - *Decision: Add `status` enum to Patient model: 'EMERGENCY', 'ACTIVE', 'ARCHIVED'.*
5.  **Event Publishing:** Publishes a `Patient.Created` event to the `app_events` queue with the new patient data.
6.  **Response:** Returns the created patient object, including the generated MRN.
7.  **Error Handling:** Returns 400 if validation fails or 500 if DB error.

## Tasks / Subtasks

- [ ] 1. Enhance Patient Model
    - [ ] Update `apps/api-server/src/patient/models.py` to include `status` Enum (`EMERGENCY`, `ACTIVE`, `ARCHIVED`).
- [ ] 2. Define Emergency Schemas
    - [ ] Create `PatientEmergencyCreate` schema in `schemas.py` (fields: `firstName`, `lastName`, `gender`).
- [ ] 3. Implement MRN Generator Utility
    - [ ] Create `apps/api-server/src/patient/utils.py`.
    - [ ] Implement `generate_mrn()` -> `str` (Format: `MRN-{YYYY}-{6_DIGITS}`).
    - [ ] Ensure uniqueness check loop (query DB to ensure no collision).
- [ ] 4. Implement Service Logic
    - [ ] Add `create_emergency_patient` method to `service.py`.
    - [ ] Logic: Generate MRN, Create Patient object with `status='EMERGENCY'`, Insert to DB, Commit.
    - [ ] Logic: Call `message_bus.publish('Patient.Created', patient_data)`.
- [ ] 5. Implement API Endpoint
    - [ ] Add `POST /emergency` to `router.py`.
    - [ ] Connect to service method.
- [ ] 6. Write Unit Tests
    - [ ] Test 1: Successful emergency registration returns MRN.
    - [ ] Test 2: Verify `status` is set to 'EMERGENCY'.
    - [ ] Test 3: Verify event is published to queue (mock message bus).

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **MRN Generation:** Use `secrets.choice` or `random.SystemRandom` for secure random generation if security is a concern, though standard `random` is fine for MRNs. Format: `MRN-2025-XXXXXX`.
-   **Transaction Management:** The DB Insert and Event Publish MUST happen within the same transaction (or ensuring eventual consistency). Ideally, publish the event *after* commit, or use the "Outbox Pattern" where the event is saved to DB in same transaction. *Architecture Decision: We are using 'Internal-as-External' via `app_events` table. This IS the Outbox pattern. So, insert Patient AND insert Event in the SAME transaction.*
    ```python
    # CORRECT PATTERN
    async with session.begin():
        session.add(new_patient)
        await message_bus.publish(session, 'Patient.Created', payload)
    # Commit happens automatically at end of block
    ```
-   **Enums:** Use Python `StrEnum` (Python 3.11+) for `Gender` and `Status` to ensure easy JSON serialization.

### Project Structure Requirements

-   Files: `apps/api-server/src/patient/utils.py` (New), `apps/api-server/src/patient/service.py` (Update), `apps/api-server/src/patient/router.py` (Update).

### References

-   [Source: _bmad-output/planning-artifacts/epics.md#story-22-emergency-registration-api--logic] (Acceptance Criteria)
-   [Source: _bmad-output/implementation-artifacts/1-2-shared-message-bus-infrastructure.md] (Message Bus Usage)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
