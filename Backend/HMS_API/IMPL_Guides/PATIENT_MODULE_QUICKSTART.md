# Patient Module Quick Start Guide

## Step 1: Verify Database Schema

Ensure `pat_patients` and `audit_logs` tables exist:

```sql
-- Verify patient table
SELECT column_name, data_type, is_nullable
FROM information_schema.columns
WHERE table_name = 'pat_patients'
ORDER BY ordinal_position;

-- Verify audit logs table
SELECT column_name, data_type, is_nullable
FROM information_schema.columns
WHERE table_name = 'audit_logs'
ORDER BY ordinal_position;
```

Expected columns for `pat_patients`:
- `id` (INT, NOT NULL, PRIMARY KEY)
- `mrn` (VARCHAR(20), UNIQUE)
- `first_name` (VARCHAR(100))
- `last_name` (VARCHAR(100))
- `gender` (VARCHAR(20))
- `dob` (DATE)
- `contact_info` (JSONB)
- `is_emergency_reg` (BOOLEAN)
- `created_at` (TIMESTAMP)

## Step 2: Configure Authentication

Ensure you have a valid JWT token:

```bash
# Login to get token
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{
    "username": "testuser",
    "password": "password123"
  }'

# Extract access token from response
export ACCESS_TOKEN="your-jwt-token-here"
```

## Step 3: Build and Run

```bash
cd Backend/HMS_API
dotnet build
cd HMS.API
dotnet run
```

## Step 4: Test Emergency Registration

Create a patient with minimal data:

```bash
curl -X POST http://localhost:5000/api/patients/emergency \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $ACCESS_TOKEN" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "gender": "M"
  }'
```

Expected response (201 Created):
```json
{
  "id": 1,
  "mrn": "MRN-2026-0001",
  "firstName": "John",
  "lastName": "Doe",
  "gender": "M",
  "dob": null,
  "contactInfo": null,
  "isEmergencyReg": true,
  "createdAt": "2025-01-05T00:00:00Z"
}
```

## Step 5: Test Patient Update

Update patient with full demographics:

```bash
MRN="MRN-2026-0001"

curl -X POST http://localhost:5000/api/patients/$MRN \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $ACCESS_TOKEN" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "dob": "1990-05-15",
    "gender": "M",
    "contactInfo": {
      "email": "john.doe@example.com",
      "phone": "555-123-4567",
      "address": "123 Main St, City, State 12345",
      "insuranceProvider": "BlueCross",
      "policyNumber": "POL-123456789"
    }
  }'
```

Expected response (200 OK):
```json
{
  "id": 1,
  "mrn": "MRN-2026-0001",
  "firstName": "John",
  "lastName": "Doe",
  "gender": "M",
  "dob": "1990-05-15",
  "contactInfo": {
    "email": "john.doe@example.com",
    "phone": "555-123-4567",
    "address": "123 Main St, City, State 12345",
    "insuranceProvider": "BlueCross",
    "policyNumber": "POL-123456789"
  },
  "isEmergencyReg": false,
  "createdAt": "2025-01-05T00:00:00Z"
}
```

## Step 6: Test Patient Retrieval

Get patient by MRN:

```bash
MRN="MRN-2026-0001"

curl -X GET http://localhost:5000/api/patients/$MRN \
  -H "Authorization: Bearer $ACCESS_TOKEN"
```

Expected response (200 OK):
```json
{
  "message": "Patient with MRN MRN-2026-0001 retrieved.",
  "data": {
    "id": 1,
    "mrn": "MRN-2026-0001",
    "firstName": "John",
    "lastName": "Doe",
    "gender": "M",
    "dob": "1990-05-15",
    "contactInfo": {
      "email": "john.doe@example.com",
      "phone": "555-123-4567",
      "address": "123 Main St, City, State 12345",
      "insuranceProvider": "BlueCross",
      "policyNumber": "POL-123456789"
    },
    "isEmergencyReg": false,
    "createdAt": "2025-01-05T00:00:00Z"
  }
}
```

## Step 7: Verify Event Publishing

Check if `Patient.Created` event was published:

```sql
SELECT id, event_type, status, created_at, payload
FROM app_events
WHERE event_type = 'Patient.Created'
ORDER BY created_at DESC
LIMIT 5;
```

Expected event payload:
```json
{
  "PatientId": 1,
  "Mrn": "MRN-2026-0001",
  "FullName": "John Doe",
  "CreatedBy": 1,
  "CreatedAt": "2025-01-05T00:00:00Z"
}
```

## Step 8: Verify Audit Logging

Check if patient update was logged:

```sql
SELECT id, entity_type, entity_id, action, user_id, created_at
FROM audit_logs
WHERE entity_type = 'Patient'
ORDER BY created_at DESC
LIMIT 5;
```

## Using the HTML Client

Open the patient registration HTML client:

```bash
# Start the API
cd Backend/HMS_API/HMS.API
dotnet run

# Open browser
# Navigate to: http://localhost:5000/patient-portal.html
```

The HTML client provides:
- Emergency registration form
- Patient update form
- Patient search by MRN
- JWT token management

## Common Issues

### 1. Emergency registration returns 401

**Cause:** Missing or invalid JWT token

**Solution:**
```bash
# Login again
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "password123"
  }'

# Update ACCESS_TOKEN variable
export ACCESS_TOKEN="new-token-here"
```

### 2. Update returns 404

**Cause:** Patient MRN not found

**Solution:**
```bash
# List all patients
psql -U postgres -d hms -c "SELECT id, mrn, first_name, last_name FROM pat_patients LIMIT 10;"

# Use correct MRN
```

### 3. Duplicate MRN generated

**Cause:** MRN generation logic issue

**Solution:**
```bash
# Check existing MRNs
psql -U postgres -d hms -c "SELECT mrn FROM pat_patients ORDER BY mrn DESC LIMIT 5;"

# The next MRN should increment the sequence
```

### 4. Contact info not saving

**Cause:** JSON serialization issue

**Solution:**
```bash
# Check contact_info column in database
psql -U postgres -d hms -c "SELECT id, contact_info FROM pat_patients WHERE mrn = 'MRN-2026-0001';"

# Verify JSON structure
```

### 5. Event not published

**Cause:** EventBus configuration or error

**Solution:**
```bash
# Check app_events table
psql -U postgres -d hms -c "SELECT * FROM app_events WHERE event_type = 'Patient.Created' ORDER BY created_at DESC LIMIT 5;"

# Check API logs for errors
```

## MRN Generation Logic

The MRN generator creates unique MRNs in format: `MRN-{YYYY}-{XXXX}`

- **YYYY**: Current year (e.g., 2026)
- **XXXX**: Sequential number, padded to 4 digits (e.g., 0001, 0002, 0003)

Example sequence:
```
MRN-2026-0001 (First patient of 2026)
MRN-2026-0002 (Second patient of 2026)
MRN-2026-0003 (Third patient of 2026)
```

The generator:
1. Queries the database for the last MRN with the current year prefix
2. Extracts the sequence number
3. Increments by 1
4. Returns formatted MRN

## Gender Values

Valid gender values:
- `M` - Male
- `F` - Female
- `O` - Other
- `U` - Unknown

## Date Format

Dates should be in ISO 8601 format: `YYYY-MM-DD`

Examples:
```json
"dob": "1990-05-15"
```

## Validation Rules

### Emergency Registration
- `firstName`: Required, string
- `lastName`: Required, string
- `gender`: Required, string (M/F/O/U)

### Update Patient
- `firstName`: Optional, string
- `lastName`: Optional, string
- `dob`: Optional, DateOnly (YYYY-MM-DD)
- `gender`: Optional, string (M/F/O/U)
- `contactInfo.email`: Optional, string
- `contactInfo.phone`: Optional, string
- `contactInfo.address`: Optional, string
- `contactInfo.insuranceProvider`: Optional, string
- `contactInfo.policyNumber`: Optional, string

## Error Response Format

All errors follow the standard format:

```json
{
  "error": {
    "code": "ERROR_CODE",
    "message": "Human readable error message",
    "details": {}
  }
}
```

Common error codes:
- `UNAUTHORIZED` - Invalid or missing JWT token
- `PATIENT_NOT_FOUND` - Patient MRN not found
- `INTERNAL_ERROR` - Server error during operation

## Testing Checklist

Before deploying to production:

- ✅ Emergency registration creates unique MRN
- ✅ Emergency registration publishes `Patient.Created` event
- ✅ Patient update stores contact info in JSONB
- ✅ Patient update creates audit log entry
- ✅ Patient retrieval returns correct data
- ✅ JWT authentication is required
- ✅ Error responses follow standard format
- ✅ MRN sequence increments correctly

## Next Steps

After patient registration is working:

1. **Implement Frontend** - Create Angular components for patient management
2. **Add Validation** - Implement FluentValidation for DTOs
3. **Add Search** - Implement patient search by name
4. **Add Pagination** - Implement list patients with pagination
5. **Add Role-Based Access** - Restrict update operations to specific roles
6. **Add Export** - Export patient data to PDF/CSV
7. **Add Photo** - Store patient photo (future enhancement)

## Resources

- **Full Implementation**: `PATIENT_MODULE_IMPLEMENTATION.md`
- **Story Details**: `Docs/stories/2-1-patient-module-database-schema.md`
- **Story Details**: `Docs/stories/2-2-emergency-registration-api-logic.md`
- **Story Details**: `Docs/stories/2-4-full-registration-demographics.md`
- **HTML Client**: `wwwroot/patient-portal.html`

## Support

If you encounter issues:

1. Check logs in console output
2. Verify database schema is correct
3. Ensure JWT token is valid
4. Check audit_logs and app_events tables
5. Verify MRN generation logic

The patient module is now ready for integration with the HMS system!
