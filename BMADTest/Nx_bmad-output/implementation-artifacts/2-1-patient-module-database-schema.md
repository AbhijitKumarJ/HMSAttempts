# Story 2.1: Patient Module & Database Schema

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Backend Developer,
I want to create the Patient module and database tables,
so that patient demographic data can be persisted securely.

## Acceptance Criteria

1.  **Module Initialized:** A new directory `apps/api-server/src/patient` is created with standard FastAPI structure (`router.py`, `schemas.py`, `models.py`, `service.py`).
2.  **Database Table:** A `pat_patients` table is defined using SQLAlchemy 2.0 `mapped_column`.
3.  **Columns Defined:** The table includes:
    -   `mrn`: String, Unique, Indexed (Primary business key).
    -   `first_name`: String.
    -   `last_name`: String.
    -   `dob`: Date.
    -   `gender`: Enum/String.
    -   `contact_info`: JSONB (for flexible phone/email/address).
    -   `created_at`: DateTime (UTC).
    -   `updated_at`: DateTime (UTC).
4.  **Data Validation:** Pydantic v2 models (`PatientCreate`, `PatientResponse`) validate inputs and use `camelCase` aliases for JSON serialization.
5.  **Migrations:** (Implicit via SQLAlchemy `Base.metadata.create_all` for MVP, or Alembic if configured).
6.  **Unit Tests:** Tests verify that a patient can be inserted and retrieved by MRN.

## Tasks / Subtasks

- [ ] 1. Initialize Patient Module
    - [ ] Create directory `apps/api-server/src/patient`
    - [ ] Create `__init__.py` exposing the router
    - [ ] Register router in `apps/api-server/src/main.py`
- [ ] 2. Define SQLAlchemy Models
    - [ ] Create `apps/api-server/src/patient/models.py`
    - [ ] Define `Patient` class inheriting from `Base`
    - [ ] Use `__tablename__ = "pat_patients"`
    - [ ] Use `mapped_column` with `Mapped` type hints for all fields
    - [ ] Implement `contact_info` as `Mapped[dict[str, Any]]` mapped to `JSONB` via `type_annotation_map` in Base
- [ ] 3. Define Pydantic Schemas
    - [ ] Create `apps/api-server/src/patient/schemas.py`
    - [ ] Define `PatientBase`, `PatientCreate`, `PatientResponse`
    - [ ] Config: `model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)`
- [ ] 4. Implement Basic Service Layer
    - [ ] Create `apps/api-server/src/patient/service.py`
    - [ ] Implement `create_patient` (async)
    - [ ] Implement `get_patient_by_mrn` (async)
- [ ] 5. Write Unit Tests
    - [ ] Test 1: Create a patient and verify DB insertion
    - [ ] Test 2: Retrieve patient by MRN
    - [ ] Test 3: Verify JSONB serialization of contact info

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **SQLAlchemy 2.0:** Use the modern declarative style.
    ```python
    class Patient(Base):
        __tablename__ = "pat_patients"
        mrn: Mapped[str] = mapped_column(String(50), unique=True, index=True)
        # ...
    ```
-   **JSONB Handling:** Ensure your `Base` class (in `database.py`) has the correct `type_annotation_map` for `dict` -> `JSONB` to avoid manual type specification in every model.
-   **Naming:**
    -   Table: `pat_patients` (snake_case, prefixed)
    -   Python fields: `first_name` (snake_case)
    -   JSON API: `firstName` (camelCase) - handled automatically by Pydantic config.

### Project Structure Requirements

-   Directory: `apps/api-server/src/patient/`
-   Files: `models.py`, `schemas.py`, `service.py`, `router.py`

### References

-   [Source: _bmad-output/planning-artifacts/epics.md#story-21-patient-module--database-schema] (Acceptance Criteria)
-   [Source: _bmad-output/planning-artifacts/architecture.md#data-architecture] (JSONB & Postgres 18.1)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
