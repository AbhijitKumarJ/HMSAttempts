# Story 3.2: Dynamic Form Builder API

**Status:** ready-for-dev

## Story

As an Admin,
I want to define clinical forms via API,
So that I can create new triage protocols without code changes.

## Acceptance Criteria

1.  **Endpoint:** `POST /api/clinical/templates` to save a form schema.
2.  **Endpoint:** `GET /api/clinical/templates/{id}` to retrieve a schema.
3.  **Format:** Schema format is JSON-compatible with `ngx-formly` or a custom renderer.
    *   *Example Structure:*
        ```json
        [
          {
            "key": "systolic_bp",
            "type": "input",
            "templateOptions": {
              "label": "Systolic BP",
              "type": "number",
              "min": 0,
              "max": 300,
              "required": true
            }
          }
        ]
        ```
4.  **Validation:** Ensure schema contains valid field definitions.

## Technical Implementation

### Controller
*   `ClinicalTemplateController`.

### Service
*   `ClinicalTemplateService` handles CRUD operations for `clin_form_templates` table.
