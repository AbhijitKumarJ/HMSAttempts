# Story 3.1: Clinical Module & Dynamic Form Schema

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Backend Developer,
I want to create the Clinical module with JSONB support,
so that we can store flexible assessment data.

## Acceptance Criteria

1.  **Module Initialized:** A new directory `apps/api-server/src/clinical` exists with standard FastAPI structure.
2.  **Database Tables:**
    -   `clin_assessments`: Stores individual patient assessments.
        -   `id`: Primary Key (BigInt/Identity).
        -   `patient_mrn`: Foreign Key to `pat_patients.mrn`.
        -   `template_id`: Foreign Key to `clin_form_templates.id`.
        -   `data`: JSONB (Stores the actual form responses).
        -   `status`: Enum ('DRAFT', 'FINAL', 'AMENDED').
        -   `created_at`, `updated_at`, `finalized_at`: Timestamps.
    -   `clin_form_templates`: Stores the definitions of dynamic forms.
        -   `id`: Primary Key (UUID/String).
        -   `title`: String (e.g., "Acute Chest Pain Protocol").
        -   `version`: String/Integer (for schema evolution).
        -   `schema`: JSONB (The JSON Schema definition).
        -   `ui_schema`: JSONB (Layout/UI hints, compatible with JSON Forms or similar).
        -   `is_active`: Boolean.
3.  **Database Constraints:**
    -   `clin_assessments.data` MUST be a valid JSON Object (enforced via `CHECK (jsonb_typeof(data) = 'object')`).
    -   (Optional/Advanced) Use `pg_jsonschema` if available to validate `data` against `clin_form_templates.schema` at the database level (otherwise validate in app layer). *Decision: Validate in App Layer (Pydantic) for MVP simplicity, add DB constraint later.*
4.  **Data Models:** Pydantic v2 models for `AssessmentCreate`, `AssessmentResponse`, `TemplateCreate`, `TemplateResponse`.
5.  **Service Layer:** Basic CRUD operations for Templates and Assessments.

## Tasks / Subtasks

- [ ] 1. Initialize Clinical Module
    - [ ] Create `apps/api-server/src/clinical/{router,schemas,models,service}.py`.
    - [ ] Register router in `main.py`.
- [ ] 2. Define SQLAlchemy Models
    - [ ] Create `clin_form_templates` model with `schema` and `ui_schema` as `Mapped[dict[str, Any]]`.
    - [ ] Create `clin_assessments` model with `data` as `Mapped[dict[str, Any]]`.
    - [ ] Add Foreign Keys and Indices (Index `patient_mrn` and `status`).
- [ ] 3. Implement JSON Schema Validation Logic
    - [ ] Add `jsonschema` python library to `pyproject.toml`.
    - [ ] In `service.py`, before saving an assessment, validate `assessment_data` against `template.schema` using `jsonschema.validate`.
    - [ ] Handle validation errors gracefully (return HTTP 422).
- [ ] 4. Create Seed Data
    - [ ] Create a migration or startup script to insert 2-3 standard templates:
        -   "General Triage" (Vitals: BP, HR, Temp, SpO2; Chief Complaint).
        -   "Acute Chest Pain" (Vitals + EKG Required + Pain Score 1-10).
- [ ] 5. Write Unit Tests
    - [ ] Test 1: Create Template (verify JSON storage).
    - [ ] Test 2: Create Assessment (verify link to patient).
    - [ ] Test 3: Validation Logic (Try submitting invalid data against a schema, ensure 422).

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **JSON Schema:** Use **Draft 7** or **2020-12** standard for schemas.
    -   Use `if-then-else` for conditional logic (e.g., IF "pain_score" > 7 THEN require "pain_location").
    -   Frontend will likely use **JSON Forms** (Angular), so structure `ui_schema` accordingly (categorization, layout).
-   **Validation:**
    -   **Strictness:** Set `additionalProperties: false` in schemas to prevent garbage data.
    -   **Layering:** Validate in Pydantic/Service layer. If performance allows, use `pg_jsonschema` later.
-   **Postgres:** Ensure `gin` index on `clin_assessments.data` to allow searching within the JSON (e.g., "Find all patients with temp > 38").

### Project Structure Requirements

-   Directory: `apps/api-server/src/clinical/`
-   Files: `models.py`, `schemas.py`, `service.py`, `router.py`, `seed_data.py`

### References

-   [Source: _bmad-output/planning-artifacts/epics.md#story-31-clinical-module--dynamic-form-schema]
-   [Source: Web Research] (JSON Schema Conditional Logic)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
