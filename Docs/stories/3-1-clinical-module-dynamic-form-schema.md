# Story 3.1: Clinical Module & Dynamic Form Schema

**Status:** ready-for-dev

## Story

As a Backend Developer,
I want to create the relational schema for dynamic forms,
So that we can build forms from reusable field definitions and maintain data integrity.

## Acceptance Criteria

1.  **Project Created:** `HMS.Entity/Clinical`.
2.  **Schema:** Create the following tables as per `DB/db_design.md`:
    *   `clin_field_definitions`: Reusable library of fields (DataType, Units, Validations).
    *   `clin_form_templates`: Form headers and versioning.
    *   `clin_form_fields`: Mapping fields to templates with specific ordering and labels.
    *   `clin_assessments`: Header for a filled instance of a form.
    *   `clin_assessment_values`: Detailed row-per-value storage (Raw + Typed JSONB).
3.  **Relationships:** Foreign keys enforce integrity between templates, fields, and assessments.
4.  **Optimizations:** Index `value_typed` (JSONB) for querying.

## Technical Implementation

### Entity Framework
*   **Entities:** Create `FieldDefinition`, `FormTemplate`, `FormField`, `Assessment`, `AssessmentValue`.
*   **Property:** `AssessmentValue.ValueTyped` uses `public JsonDocument` mapped to `jsonb`.
*   **Migration:** Run `dotnet ef migrations add ClinicalRelationalSchema`.
