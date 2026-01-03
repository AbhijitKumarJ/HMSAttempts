# Database Design for HMS

## Tables

### Users
- **UserID** (Primary Key, INT, Auto Increment)
- **Username** (VARCHAR, Unique)
- **PasswordHash** (VARCHAR)
- **Role** (ENUM: 'Doctor', 'Nurse', 'Staff', 'Admin', 'Billing Clerk')
- **CreatedAt** (DATETIME)
- **UpdatedAt** (DATETIME)

### Patients
- **PatientID** (Primary Key, INT, Auto Increment)
- **MRN** (VARCHAR, Unique)
- **FirstName** (VARCHAR)
- **LastName** (VARCHAR)
- **DateOfBirth** (DATE)
- **Gender** (ENUM: 'Male', 'Female', 'Other')
- **ContactInfo** (JSONB)

### Roles
- **RoleID** (Primary Key, INT, Auto Increment)
- **RoleName** (VARCHAR, Unique)

### Appointments
- **AppointmentID** (Primary Key, INT, Auto Increment)
- **PatientID** (Foreign Key)
- **DoctorID** (Foreign Key)
- **AppointmentDate** (DATETIME)
- **Status** (ENUM: 'Scheduled', 'Completed', 'Cancelled')

### Assessments
- **AssessmentID** (Primary Key, INT, Auto Increment)
- **ConsultationID** (Foreign Key, INT)
- **DynamicFormID** (Foreign Key, INT)
- **AssessmentDate** (DATETIME)
- **Summary** (TEXT)

### FieldDefinitions
- **FieldID** (Primary Key, INT, Auto Increment)
- **FieldCode** (VARCHAR, Unique) -- stable programmatic identifier (e.g. 'systolic_bp')
- **FieldName** (VARCHAR) -- human-friendly default label
- **DataType** (ENUM: 'Integer', 'Decimal', 'String', 'Date', 'Boolean') -- semantic type
- **Unit** (VARCHAR, Nullable)
- **AllowedValues** (JSONB, Nullable) -- enumerations or option list when applicable
- **ValidationRules** (JSONB, Nullable) -- min/max/regex/etc.
- **IsRepeatable** (BOOLEAN) -- whether multiple values may be captured per assessment
- **CreatedAt** (DATETIME)
- **UpdatedAt** (DATETIME)

### FormFields (mapping)
- **FormFieldID** (Primary Key, INT, Auto Increment)
- **DynamicFormID** (Foreign Key, INT)
- **FieldID** (Foreign Key, INT) -- references `FieldDefinitions`
- **LabelOverride** (VARCHAR, Nullable) -- form-specific label
- **DisplayOrder** (INT)
- **IsRequiredOverride** (BOOLEAN, Nullable)
- **VisibilityCondition** (JSONB, Nullable) -- conditional display rules
- **UIControlType** (ENUM: 'Textbox', 'Textarea', 'Dropdown', 'Radio', 'Checkbox', 'DatePicker', 'NumberSpinner') -- how the field is rendered
- **UIOptions** (JSONB, Nullable) -- UI-specific options (e.g. dropdown option labels/values)

### AssessmentFieldValues
- **AssessmentFieldValueID** (Primary Key, INT, Auto Increment)
- **AssessmentID** (Foreign Key, INT)
- **FieldID** (Foreign Key, INT)
- **FormFieldID** (Foreign Key, INT, Nullable)
- **RawValue** (TEXT) -- raw input as captured from the UI
- **TypedValue** (JSONB, Nullable) -- canonical typed representation (e.g. { "number": 120 } )
- **Unit** (VARCHAR, Nullable)
- **RecordedByUserID** (Foreign Key, INT, Nullable)
- **RecordedAt** (DATETIME)

### Notes on DataType vs UIControlType
- Keep semantic type (`DataType`) in `FieldDefinitions` separate from `UIControlType` in `FormFields`.
- Example: a field with `DataType = Integer` may be rendered with `UIControlType = Dropdown` or `Textbox`. The UI may send `RawValue = "120"` but `TypedValue` should store the numeric representation (e.g. `{ "number": 120 }`).
- Store enumerations/options in `AllowedValues` (semantic values) and `UIOptions` (presentation labels/ordering) so the same field can be reused across forms with different UI choices.
- Use `AssessmentFieldValues` to persist captured values in a way that preserves original input and provides a canonical typed form for queries and reporting.

### Vitals
- **VitalID** (Primary Key, INT, Auto Increment)
- **PatientID** (Foreign Key)
- **BloodPressure** (VARCHAR)
- **HeartRate** (INT)
- **Temperature** (FLOAT)
- **RecordedAt** (DATETIME)

### Medications
- **MedicationID** (Primary Key, INT, Auto Increment)
- **PatientID** (Foreign Key)
- **MedicationName** (VARCHAR)
- **Dosage** (VARCHAR)
- **StartDate** (DATETIME)
- **EndDate** (DATETIME)

### Billing
- **BillingID** (Primary Key, INT, Auto Increment)
- **PatientID** (Foreign Key)
- **Amount** (DECIMAL)
- **Status** (ENUM: 'Paid', 'Pending', 'Cancelled')
- **CreatedAt** (DATETIME)

## Notes

### Labs
- **LabID** (Primary Key, INT, Auto Increment)
- **LabName** (VARCHAR)
- **Description** (TEXT)
- **CreatedAt** (DATETIME)
- **UpdatedAt** (DATETIME)

### Orders
- **OrderID** (Primary Key, INT, Auto Increment)
- **PatientID** (Foreign Key, INT)
- **LabID** (Foreign Key, INT)
- **OrderDate** (DATETIME)
- **Status** (ENUM: 'Pending', 'Completed', 'Cancelled')

### Consultations
- **ConsultationID** (Primary Key, INT, Auto Increment)
- **PatientID** (Foreign Key, INT)
- **DoctorID** (Foreign Key, INT)
- **EpisodeID** (Foreign Key, INT)
- **EncounterDate** (DATETIME)
- **Notes** (TEXT)

### Episodes
- **EpisodeID** (Primary Key, INT, Auto Increment)
- **PatientID** (Foreign Key, INT)
- **StartDate** (DATETIME)
- **EndDate** (DATETIME)
- **Diagnosis** (VARCHAR)

### Assessments
- **AssessmentID** (Primary Key, INT, Auto Increment)
- **ConsultationID** (Foreign Key, INT)
- **DynamicFormID** (Foreign Key, INT)
- **AssessmentDate** (DATETIME)
- **Results** (JSONB)

### DynamicForms
- **DynamicFormID** (Primary Key, INT, Auto Increment)
- **FormName** (VARCHAR)
- **Fields** (JSONB)

### Fields
- **FieldID** (Primary Key, INT, Auto Increment)
- **DynamicFormID** (Foreign Key, INT)
- **FieldName** (VARCHAR)
- **FieldType** (ENUM: 'Text', 'Number', 'Date', 'Boolean')
- **IsRequired** (BOOLEAN)
- The design supports multi-role functionality and dynamic role switching as per the requirements.
- Patient records are uniquely identified by MRN, ensuring no duplicates.
- Vitals and medications are linked to patients for comprehensive tracking.