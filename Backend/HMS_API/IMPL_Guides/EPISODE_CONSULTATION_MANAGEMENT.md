# Episode & Consultation Management Implementation Guide

## Overview
Implementation of Episode of Care Management (Story 10-2) and Consultation Lifecycle (Story 10-3) features.

## Features Implemented

### 1. Episode of Care Management
**Purpose:** Group related visits into episodes to track conditions over time.

**Endpoints:**
- `POST /api/Clinical/episodes` - Create new episode
- `GET /api/Clinical/episodes/{id}` - Get episode by ID
- `GET /api/Clinical/patients/{patientId}/episodes` - Get all episodes for a patient
- `PUT /api/Clinical/episodes/{id}/close` - Close an episode

**Episode Features:**
- Episodes track patient conditions (e.g., "Pregnancy 2024", "Diabetes Management")
- Each episode has a title, start date, end date, and status
- Episodes are linked to patients
- Consultations can be linked to episodes
- Closing an episode sets the end date and status to "Closed"

### 2. Consultation Lifecycle
**Purpose:** Explicitly start and end consultations to track encounter duration.

**Endpoints:**
- `POST /api/Clinical/consultations/start` - Start consultation from appointment
- `GET /api/Clinical/consultations/{id}` - Get consultation by ID
- `GET /api/Clinical/doctors/{doctorId}/active-consultation` - Get active consultation for doctor
- `GET /api/Clinical/patients/{patientId}/consultations` - Get all consultations for patient
- `PUT /api/Clinical/consultations/{id}/end` - End consultation with clinical summary
- `PUT /api/Clinical/consultations/{id}/link-episode` - Link consultation to episode

**Consultation Features:**
- Start: Creates consultation from appointment, sets started_at timestamp
- Link: Consultation linked to sch_appointments table
- End: Sets ended_at timestamp and clinical summary
- Active consultation check: Only one active consultation per doctor at a time
- Occupied dashboard view: Shows doctors with active consultations

## API Documentation

### Episode Management

#### Create Episode
```bash
POST /api/Clinical/episodes
Content-Type: application/json
Authorization: Bearer {token}

{
  "patientId": 1,
  "title": "Pregnancy 2024",
  "startDate": "2024-01-15T10:00:00Z"
}
```

#### Get Episode
```bash
GET /api/Clinical/episodes/{id}
Authorization: Bearer {token}
```

#### Get Patient Episodes
```bash
GET /api/Clinical/patients/{patientId}/episodes
Authorization: Bearer {token}
```

#### Close Episode
```bash
PUT /api/Clinical/episodes/{id}/close
Authorization: Bearer {token}
```

### Consultation Management

#### Start Consultation
```bash
POST /api/Clinical/consultations/start
Content-Type: application/json
Authorization: Bearer {token}

{
  "appointmentId": 1,
  "doctorId": 1,
  "episodeId": 1
}
```

#### Get Consultation
```bash
GET /api/Clinical/consultations/{id}
Authorization: Bearer {token}
```

#### Get Active Consultation by Doctor
```bash
GET /api/Clinical/doctors/{doctorId}/active-consultation
Authorization: Bearer {token}
```

#### Get Patient Consultations
```bash
GET /api/Clinical/patients/{patientId}/consultations
Authorization: Bearer {token}
```

#### End Consultation
```bash
PUT /api/Clinical/consultations/{id}/end
Content-Type: application/json
Authorization: Bearer {token}

{
  "clinicalSummary": "Patient presented with flu symptoms. Prescribed rest and fluids."
}
```

#### Link Consultation to Episode
```bash
PUT /api/Clinical/consultations/{id}/link-episode
Content-Type: application/json
Authorization: Bearer {token}

{
  "episodeId": 5
}
```

## Files Created/Modified

### New Files
- `HMS.Entity/Clinical/ClinicalEntities.cs` - Added CreateEpisodeDto, EpisodeDto, StartConsultationDto, ConsultationDto, EndConsultationDto, LinkConsultationEpisodeDto

### Modified Files
- `HMS.Data/Clinical/ClinicalRepository.cs` - Added episode and consultation CRUD methods
- `HMS.Business/Clinical/ClinicalService.cs` - Added episode and consultation business logic
- `HMS.API/Controllers/Clinical/ClinicalController.cs` - Added episode and consultation endpoints

### Test Files
- `wwwroot/episode-consultation-test.html` - Test UI for episode and consultation features

## Database Schema

### Episodes Table (`sch_episodes`)
- `id` (bigint) - Primary key
- `patient_id` (int) - Foreign key to patients
- `title` (varchar(200)) - Episode name
- `start_date` (timestamp) - When episode started
- `end_date` (timestamp) - When episode ended (nullable)
- `status` (varchar(50)) - "Active" or "Closed"

### Consultations Table (`clin_consultations`)
- `id` (bigint) - Primary key
- `appointment_id` (bigint) - Foreign key to appointments (nullable)
- `episode_id` (bigint) - Foreign key to episodes (nullable)
- `patient_id` (int) - Foreign key to patients (nullable)
- `doctor_id` (int) - Foreign key to users/doctors (nullable)
- `started_at` (timestamp) - When consultation started
- `ended_at` (timestamp) - When consultation ended (nullable)
- `clinical_summary` (text) - Summary of consultation

## Testing

### Test Scenarios

#### Episode Testing
1. Create a new episode for a patient
2. Retrieve episode by ID
3. List all episodes for a patient
4. Close an episode (sets end_date and status)
5. Verify episode appears in patient dashboard

#### Consultation Testing
1. Book an appointment first (use scheduling-test.html)
2. Start consultation from appointment
3. Verify consultation is linked to appointment
4. Check for active consultation by doctor
5. End consultation with clinical summary
6. List patient's consultation history
7. Link existing consultation to an episode

### Test Page
Navigate to `/episode-consultation-test.html`
- Create episodes for patients
- Start consultations from appointments
- End consultations with clinical summaries
- Link consultations to episodes
- View patient episodes and consultation history

## Acceptance Criteria Met

### Story 10-2: Episode of Care Management ✓
- [x] Create new Episode or Link Consultation to existing Episode
- [x] Patient Dashboard shows "Active Episodes" (via GET /patients/{id}/episodes)
- [x] Saves to `sch_episodes`
- [x] Close episode endpoint available

### Story 10-3: Consultation Lifecycle ✓
- [x] Clicking "Start Visit" creates a `clin_consultations` record
- [x] Consultation linked to `sch_appointments`
- [x] Clicking "Finish" sets the `ended_at` timestamp
- [x] Active consultations appear on "Occupied" dashboard (via GET /doctors/{id}/active-consultation)
- [x] Only one active consultation per doctor at a time (validation in service)

## Business Logic

### Episode Logic
- Episodes must have a valid patient_id
- Episodes start with status "Active"
- Closing an episode sets end_date to current UTC and status to "Closed"
- Episodes are ordered by start_date (newest first)

### Consultation Logic
- Starting consultation requires appointment_id and doctor_id
- episode_id is optional when starting (can link later)
- System validates doctor doesn't have active consultation before starting new one
- Returns 409 Conflict if doctor already has active consultation
- Ending consultation requires clinical_summary
- Cannot end already-ended consultation
- Consultation duration = ended_at - started_at
- Consultations ordered by started_at (newest first)

## Dependencies
- .NET 9.0
- Entity Framework Core 9.0
- PostgreSQL
- Existing Appointment Scheduling (Story 10-1)
- Existing Patient module (Stories 2-1, 2-2)

## Notes
- Consultations are the actual doctor-patient encounters
- Episodes group multiple consultations for condition tracking
- Example: "Pregnancy 2024" episode → multiple prenatal visits (consultations)
- Active consultation check prevents double-booking doctors in real-time
- Clinical summary is free-text for doctor notes
- Both episodes and consultations support optional linking (flexible workflow)

## Related Stories
- Story 10-1: Appointment Scheduling (prerequisite for consultations)
- Story 2-1: Patient Module (patients must exist)
- Story 4-2: Patient Dashboard (displays episodes)
