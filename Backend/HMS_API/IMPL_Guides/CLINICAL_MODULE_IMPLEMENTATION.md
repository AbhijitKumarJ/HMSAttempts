# Clinical Module Implementation Guide

## Overview
The Clinical Module provides dynamic form management and vital signs tracking functionality for the Hospital Management System.

## Stories Implemented
- Story 3.1: Clinical Module & Dynamic Form Schema
- Story 3.2: Dynamic Form Builder API
- Story 3.3: Triage Workflow & Vitals Capture
- Story 3.4: Vital Signs Alerting Logic

## Architecture

### Entity Layer (HMS.Entity/Clinical)
- `FieldDefinitionDto` - DTO for reusable field definitions
- `CreateFieldDefinitionDto` - DTO for creating field definitions
- `FormFieldDto` - DTO for form field mappings
- `CreateFormFieldDto` - DTO for creating form fields
- `FormTemplateDto` - DTO for form templates with fields
- `CreateFormTemplateDto` - DTO for creating form templates
- `AssessmentValueDto` - DTO for assessment values
- `CreateAssessmentValueDto` - DTO for creating assessment values
- `AssessmentDto` - DTO for completed assessments
- `CreateAssessmentDto` - DTO for creating assessments
- `VitalsCaptureDto` - DTO for vital signs capture
- `VitalsValidationResult` - DTO for validation alerts
- `VitalsValidationResponse` - DTO for validation response
- `VitalsAlertRule` - DTO for alert threshold rules

### Data Layer (HMS.Data/DBModel)
- `ClinFieldDefinition` - Reusable field definitions
- `ClinFormTemplate` - Form templates with versioning
- `ClinFormField` - Template-to-field mapping
- `ClinAssessment` - Assessment header records
- `ClinAssessmentValue` - Assessment values with JSONB storage
- `ClinVital` - Vital signs records

### Repository Layer (HMS.Data/Clinical)
- `IClinicalRepository` - Repository interface
- `ClinicalRepository` - Implementation with methods:
  - `CreateFormTemplate`, `GetFormTemplate`, `GetFormTemplates`, `DeleteFormTemplate`
  - `CreateVital`, `GetVital`, `GetPatientVitals`

### Service Layer (HMS.Business/Clinical)
- `IClinicalService` - Service interface
- `ClinicalService` - Implementation with business logic:
  - Template CRUD operations
  - Vitals CRUD operations
  - Vitals validation with threshold rules

### Controller Layer (HMS.API/Controllers/Clinical)
- `ClinicalController` - REST API endpoints

## API Endpoints

### Dynamic Form Builder
- `POST /api/clinical/templates` - Create form template
- `GET /api/clinical/templates` - List all templates
- `GET /api/clinical/templates/{id}` - Get specific template
- `DELETE /api/clinical/templates/{id}` - Delete template

### Vitals Capture
- `POST /api/clinical/vitals` - Record patient vitals
- `GET /api/clinical/vitals/{id}` - Get specific vital reading
- `GET /api/clinical/patients/{patientId}/vitals` - Get patient vitals history
- `POST /api/clinical/vitals/validate` - Validate vitals data

### Alert Rules
- `GET /api/clinical/vitals/alert-rules` - Get validation threshold rules

## Vitals Validation Thresholds

### Blood Pressure (Systolic)
- Critical: >180 mmHg (Hypertensive Crisis)
- Critical: <90 mmHg (Hypotension)
- Warning: >140 mmHg (Hypertension Stage 1)

### Blood Pressure (Diastolic)
- Critical: >110 mmHg (Hypertensive Crisis)
- Warning: <60 mmHg

### Heart Rate
- Critical: >120 bpm (Tachycardia)
- Critical: <50 bpm (Bradycardia)
- Warning: >100 bpm

### Temperature
- Critical: >39°C (High Fever)
- Critical: <35°C (Hypothermia)
- Warning: >38°C (Fever)

### SpO2 (Oxygen Saturation)
- Critical: <90% (Severe Hypoxemia)
- Warning: <94% (Mild Hypoxemia)

## JSONB Storage
`ClinAssessmentValue.ValueTyped` uses `JsonDocument` type mapped to PostgreSQL JSONB for:
- Efficient storage of typed values
- Indexable for queries
- Flexible for different data types

## Testing
Use `clinical-test.html` for API testing and validation.
