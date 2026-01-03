# Story 3.2: Dynamic Form Builder API

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As an Admin,
I want to define clinical forms via API,
so that I can create new triage protocols without code changes.

## Acceptance Criteria

1.  **Template Creation Endpoint:** `POST /clinical/templates` accepts a JSON payload defining a new form template.
2.  **Schema Validation:** The API validates that the submitted `schema` is a structurally valid JSON Schema (Draft 7 or newer).
3.  **Unique Identification:** Each template is assigned a unique ID (UUID) and stored in the `clin_form_templates` table.
4.  **Versioning:** If a template with the same `title` is submitted, the system increments the `version` number or handles it as a new version entry.
5.  **Retrieval:** `GET /clinical/templates/{id}` retrieves the full template definition (title, version, schema, ui_schema).
6.  **Listing:** `GET /clinical/templates` returns a list of all active templates.
7.  **Deactivation:** `DELETE /clinical/templates/{id}` performs a soft delete (sets `is_active=false`).

## Tasks / Subtasks

- [ ] 1. Define Pydantic API Schemas
    - [ ] Update `apps/api-server/src/clinical/schemas.py`.
    - [ ] Create `TemplateCreate` with fields: `title`, `schema` (dict), `uiSchema` (dict).
    - [ ] Create `TemplateResponse` including `id`, `version`, and timestamps.
    - [ ] Ensure `camelCase` for JSON keys (e.g., `uiSchema`).
- [ ] 2. Implement JSON Schema Meta-Validation
    - [ ] In `service.py`, use the `jsonschema` library's `Draft7Validator.check_schema()` to verify the incoming `schema` field.
    - [ ] Raise `HTTPException(status_code=422)` with details if the schema itself is invalid.
- [ ] 3. Implement Template Service Logic
    - [ ] `create_template`: Check for existing titles to handle versioning.
    - [ ] `get_template_by_id`: Fetch from DB, handle 404.
    - [ ] `list_templates`: Fetch all where `is_active=true`.
    - [ ] `deactivate_template`: Update `is_active` flag.
- [ ] 4. Create API Router Endpoints
    - [ ] Update `apps/api-server/src/clinical/router.py`.
    - [ ] Add `POST /templates`, `GET /templates`, `GET /templates/{id}`, `DELETE /templates/{id}`.
    - [ ] Apply Admin RBAC (use `RoleGuard` if implemented, otherwise stub for now).
- [ ] 5. Write Integration Tests
    - [ ] Test 1: POST a valid schema and verify 201 Created.
    - [ ] Test 2: POST an invalid JSON Schema (e.g., missing type) and verify 422.
    - [ ] Test 3: Verify version incrementing logic for same-title templates.
    - [ ] Test 4: Verify soft-delete removes template from list but not DB.

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **FastAPI Modularity:** Use `APIRouter` with a prefix (e.g., `/clinical`).
-   **JSON Schema Extras:** When storing schemas, ensure you preserve keys needed for frontend renderers (like `jsonforms`).
-   **Naming Consistency:**
    -   API: `camelCase` (`uiSchema`, `templateId`).
    -   DB/Python: `snake_case` (`ui_schema`, `template_id`).
-   **Security:** Ensure the `schema` field is not vulnerable to injection if used in dynamic queries (unlikely with JSONB but worth noting).

### Project Structure Requirements

-   Files: `apps/api-server/src/clinical/router.py`, `apps/api-server/src/clinical/service.py`, `apps/api-server/src/clinical/schemas.py`.

### References

-   [Source: _bmad-output/planning-artifacts/epics.md#story-32-dynamic-form-builder-api]
-   [Source: _bmad-output/implementation-artifacts/3-1-clinical-module-dynamic-form-schema.md] (Base Schema)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
