# HMS API - Patient Search & Appointment Scheduling

## Overview
Implementation of Patient Search (Story 9-2) and Appointment Scheduling (Story 10-1) features.

## Features Implemented

### 1. Patient Search API
**Endpoints:**
- `GET /api/patients` - Get all patients (paginated via repository)
- `GET /api/patients/search?q={query}` - Fuzzy search by name or MRN
- `GET /api/patients/{mrn}` - Get patient by MRN (existing)

**Search Features:**
- Case-insensitive fuzzy search using PostgreSQL `ILike`
- Searches across MRN, first name, and last name
- Returns top 10 results ordered by name
- All patients endpoint returns full list ordered by name

### 2. Appointment Scheduling API
**Endpoints:**
- `GET /api/appointments` - Search appointments with filters
- `POST /api/appointments` - Book new appointment
- `GET /api/appointments/{id}` - Get appointment by ID

**Appointment Search Features:**
- **Start Date** (optional): Defaults to today if not provided
- **End Date** (optional): If provided, includes all appointments in date range
- **Doctor ID** (optional): If provided, filters by specific doctor; if not, returns all doctors
- Results sorted by appointment date

**Booking Features:**
- Validates patient exists
- Validates doctor exists
- **Double-booking prevention**: Checks doctor availability before booking
- Returns 409 Conflict if doctor already booked at same time
- Includes patient and doctor names in response

## API Documentation

### Doctors API
```bash
# Get all doctors
GET /api/auth/doctors
Authorization: Bearer {token}

# Search doctors by name or ID
GET /api/auth/doctors?q=smith
Authorization: Bearer {token}
```

### Patient Search
```bash
# Get all patients
GET /api/patients
Authorization: Bearer {token}

# Search patients
GET /api/patients/search?q=John
Authorization: Bearer {token}
```

### Appointment Search
```bash
# Search for today's appointments (all doctors)
GET /api/appointments
Authorization: Bearer {token}

# Search for date range
GET /api/appointments?startDate=2026-01-07&endDate=2026-01-10
Authorization: Bearer {token}

# Search for specific doctor on date
GET /api/appointments?startDate=2026-01-07&doctorId=1
Authorization: Bearer {token}

# Search for date range + doctor
GET /api/appointments?startDate=2026-01-07&endDate=2026-01-10&doctorId=2
Authorization: Bearer {token}
```

### Book Appointment
```bash
POST /api/appointments
Content-Type: application/json
Authorization: Bearer {token}

{
  "patientId": 1,
  "doctorId": 1,
  "appointmentDate": "2026-01-20T10:00:00Z",
  "reasonForVisit": "Annual checkup"
}
```

## Files Created/Modified

### New Files
- `HMS.Entity/Patient/PatientSearchResultDto.cs` - Patient search response DTO
- `HMS.Entity/Scheduling/AppointmentDto.cs` - Appointment request/response DTOs
- `HMS.Data/Scheduling/SchedulingRepository.cs` - Appointment data access layer
- `HMS.Business/Scheduling/SchedulingService.cs` - Appointment business logic
- `HMS.API/Controllers/Scheduling/AppointmentController.cs` - Appointment API endpoints

### Modified Files
- `HMS.Data/Patient/PatientRepository.cs` - Added search methods
- `HMS.Business/Patient/PatientService.cs` - Added search methods
- `HMS.API/Controllers/Patient/PatientController.cs` - Added search endpoints
- `HMS.API/Program.cs` - Registered scheduling services

### Test Files
- `wwwroot/patient-portal.html` - Updated with search UI
- `wwwroot/scheduling-test.html` - New appointment booking UI
- `wwwroot/test-data.sql` - SQL script for sample data
- `wwwroot/TESTING_GUIDE.md` - Comprehensive testing documentation

## Testing

### Setup Test Data
Run the SQL script to populate test data:
```bash
psql -U postgres -d hms -f wwwroot/test-data.sql
```

### Test Pages
1. **Index Page**: `http://localhost:5000/` - Access all test pages
2. **Patient Portal**: Test patient registration, search, and retrieval
3. **Scheduling Test**: Test appointment booking and search
4. **Testing Guide**: `http://localhost:5000/TESTING_GUIDE.md` - Full testing scenarios

## Acceptance Criteria Met

### Story 9-2: Patient Search ✓
- [x] `GET /api/search?q=...` endpoint exists
- [x] Fuzzy search on Patients (Name, MRN)
- [x] SearchService uses EF.Functions.ILike for Postgres
- [x] Top 10 results returned

### Story 10-1: Appointment Scheduling ✓
- [x] User can select Patient, Doctor, Date, Time
- [x] System validates doctor availability (double-booking check)
- [x] Saves to `sch_appointments` table
- [x] `GET /api/appointments` with date range filtering
- [x] Optional doctor_id filter
- [x] Start date defaults to today

## Dependencies
- .NET 9.0
- Entity Framework Core 9.0
- PostgreSQL with pgcrypto extension
- JWT Authentication

## Notes
- Password hashes in test data are placeholders - replace with actual bcrypt hashes
- Appointment dates in SQL use relative dates - adjust `NOW()` offsets as needed
- All endpoints require JWT authentication via `Authorization: Bearer {token}` header
