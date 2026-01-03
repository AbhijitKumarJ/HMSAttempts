# Story 2.4: Full Registration & Demographics

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Receptionist,
I want to update an existing patient record with full demographics,
so that we have complete contact and insurance information.

## Acceptance Criteria

1.  **Patient Selection:** User can search/select an existing patient (e.g., from the active queue or emergency list).
2.  **Full Demographic Form:** A detailed form allows updating:
    -   Address (Line 1, City, State, Postal Code, Country)
    -   Contact Details (Phone, Email - validated via regex)
    -   Insurance/Coverage Info (Payor Name, Plan Name, Subscriber ID, Group Number)
3.  **API Integration:** The frontend sends a `PATCH` request to `/api/patients/{mrn}`.
4.  **Backend Persistence:**
    -   Updates the `pat_patients` record.
    -   Demographic/Contact data is stored in the `contact_info` JSONB column (FHIR-aligned).
    -   Insurance data is stored in a structured format (either a new `pat_coverage` table or within `contact_info` - *Decision: Use a dedicated `pat_coverage` table for relational integrity*).
5.  **Audit Logging:**
    -   Every change triggers an entry in the immutable `audit_logs` table.
    -   Log entry includes: `user_id`, `timestamp`, `action="UPDATE"`, `resource="Patient"`, `resource_id={mrn}`, and `details` (JSONB showing old vs. new values).
6.  **Optimistic UI:** The UI reflects the saved status immediately with a "Synced" badge.
7.  **Data Integrity:** Prevent duplicate insurance entries for the same patient/payor if applicable.

## Tasks / Subtasks

- [ ] 1. Backend: Enhance Database Schema
    - [ ] Create `pat_coverage` table (id, patient_mrn, payor_name, plan_name, subscriber_id, group_number, is_active).
    - [ ] Create `audit_logs` table (id, timestamp, user_id, action, resource_type, resource_id, change_details).
- [ ] 2. Backend: Implement Update Logic
    - [ ] Create `PatientUpdate` Pydantic schema (Optional fields for all).
    - [ ] Update `service.py`: Implement `update_patient` with transaction handling.
    - [ ] **Critical:** Implement Audit Middleware or Service utility to record changes within the same transaction.
- [ ] 3. Backend: Implement PATCH Endpoint
    - [ ] Add `PATCH /patients/{mrn}` to `router.py`.
    - [ ] Ensure RBAC (Role: Receptionist or Admin).
- [ ] 4. Frontend: Create Full Registration Component
    - [ ] Generate `patient/full-registration` component.
    - [ ] Implement Reactive Form with sub-groups for "Contact" and "Insurance".
    - [ ] Add field-level validation (Email regex, required fields).
- [ ] 5. Frontend: Integrate with Patient Service
    - [ ] Add `updatePatient(mrn, data)` to `PatientService`.
    - [ ] Handle optimistic updates and success toasts.
- [ ] 6. Unit & Integration Tests
    - [ ] Test PATCH endpoint with partial data.
    - [ ] Verify `audit_logs` entry after successful update.
    - [ ] Test form validation errors.

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **FHIR Alignment:** Follow the FHIR `Patient` and `Coverage` resource structures for JSONB/Table fields.
    ```json
    // contact_info example
    {
      "telecom": [{"system": "phone", "value": "555-1234"}, {"system": "email", "value": "test@ex.com"}],
      "address": [{"line": ["123 Main St"], "city": "London", "postalCode": "SW1"}]
    }
    ```
-   **Immutable Auditing:** The `audit_logs` table must be APPEND-ONLY. Use FastAPI `BackgroundTasks` if performance becomes an issue, but for clinical integrity, synchronous logging within the DB transaction is preferred for MVP.
-   **SQLAlchemy 2.0:** Use `mapped_column` and `Mapped` type hints.
-   **Angular 17:** Standalone components, `OnPush` strategy, Signals for form status.

### Project Structure Notes

-   Backend: `apps/api-server/src/patient/`, `apps/api-server/src/common/audit.py`
-   Frontend: `apps/web-client/src/app/patient/full-registration/`

### References

-   [Source: _bmad-output/planning-artifacts/epics.md#story-24-full-registration--demographics]
-   [Source: _bmad-output/planning-artifacts/architecture.md#authentication--security] (Audit Requirement)
-   [Source: FHIR Patient Resource] (https://www.hl7.org/fhir/patient.html)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
