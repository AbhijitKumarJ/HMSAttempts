# Story 3.3: Triage Workflow & Vitals Capture

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Nurse,
I want to record patient vitals during triage using a dynamic form,
so that the doctor has baseline data for clinical decision-making.

## Acceptance Criteria

1.  **Form Rendering:** The frontend renders a dynamic form based on the selected `clin_form_templates` (e.g., "General Triage").
2.  **Vitals Capture:** Specific support for standard vitals (BP, HR, Resp Rate, Temp, SpO2) using the FHIR Observation structure.
3.  **Validation:** Input fields enforce clinical ranges (e.g., HR 0-300) defined in the JSON Schema.
4.  **Submission:** Submitting the form sends a `POST` request to `/api/clinical/assessments` with the form data payload.
5.  **FHIR Alignment:** The backend transforms the simplified JSON form data into FHIR-compliant `Observation` resources (internally or for export) or stores it in a structure that maps to FHIR. *Decision: Store as FHIR-aligned JSONB in `clin_assessments.data`.*
6.  **Draft Support:** User can "Save as Draft" to store partial progress without full validation.
7.  **UX Feedback:** Real-time visual feedback for abnormal values (Amber/Red borders) based on the schema's "warning" logic (if supported by UI schema) or post-input validation.

## Tasks / Subtasks

- [ ] 1. Backend: Enhance Assessment Service
    - [ ] Update `apps/api-server/src/clinical/service.py`.
    - [ ] Implement `create_assessment(patient_mrn, template_id, data, status)`.
    - [ ] Add logic to fetch the Template's schema and validate `data` (skip validation if status='DRAFT').
- [ ] 2. Frontend: Install JSON Forms
    - [ ] Run `npm install @jsonforms/angular @jsonforms/angular-material` in `apps/web-client`.
    - [ ] Configure `JsonFormsModule` in the application config/imports.
- [ ] 3. Frontend: Create Triage Component
    - [ ] Generate `clinical/triage-form` component.
    - [ ] Fetch the "General Triage" template (schema + UI schema) from `/api/clinical/templates`.
    - [ ] Render the form using `<jsonforms>`.
- [ ] 4. Frontend: Implement Save Actions
    - [ ] "Save Draft": Calls API with `status='DRAFT'`.
    - [ ] "Finalize": Calls API with `status='FINAL'` (triggers backend schema validation).
- [ ] 5. Backend: FHIR Transformation Utility (Optional/Stub)
    - [ ] Create `apps/api-server/src/clinical/fhir_mapper.py`.
    - [ ] Implement a helper to map flat form fields (e.g., `heart_rate`) to FHIR Observation structure (Category, Code, ValueQuantity).
- [ ] 6. Unit Tests
    - [ ] Test form rendering with mock schema.
    - [ ] Test submission of valid and invalid data.
    - [ ] Test "Draft" persistence allows invalid data.

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **Library Usage:** Use **@jsonforms/angular** for the dynamic form. Do NOT build a custom form builder from scratch.
-   **Schema Structure:** The `clin_form_templates.schema` MUST follow JSON Schema Draft 7 standards.
-   **FHIR Mapping:** Store data in a structure that is easy to query (e.g., flat keys in JSONB), but ensure the *schema* defines the FHIR codes (LOINC) in the `description` or `metadata` of fields for future interoperability.
    ```json
    // Schema Example
    "heart_rate": {
      "type": "integer",
      "minimum": 0,
      "maximum": 300,
      "description": "LOINC: 8867-4 | Heart rate"
    }
    ```
-   **Styling:** Use Angular Material renderers for JSON Forms to match the rest of the app (`@jsonforms/angular-material`).

### Project Structure Requirements

-   Backend: `apps/api-server/src/clinical/`
-   Frontend: `apps/web-client/src/app/clinical/triage-form/`

### References

-   [Source: _bmad-output/planning-artifacts/epics.md#story-33-triage-workflow--vitals-capture]
-   [Source: Web Research] (FHIR Vitals Panel Structure)
-   [Source: Web Research] (JSON Forms Angular Library)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
