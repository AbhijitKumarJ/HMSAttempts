# Episode & Consultation Management Quickstart Guide

## Prerequisites
- HMS.API running on `http://localhost:5000`
- PostgreSQL database configured and running
- Existing patient records in database
- Appointment Scheduling (Story 10-1) working

## Quick Start - Episodes

### 1. Create an Episode
```bash
curl -X POST http://localhost:5000/api/Clinical/episodes \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "patientId": 1,
    "title": "Pregnancy 2024",
    "startDate": "2024-01-15T10:00:00Z"
  }'
```

**Response:**
```json
{
  "id": 1,
  "patientId": 1,
  "title": "Pregnancy 2024",
  "startDate": "2024-01-15T10:00:00Z",
  "endDate": null,
  "status": "Active"
}
```

### 2. Get Patient Episodes
```bash
curl http://localhost:5000/api/Clinical/patients/1/episodes \
  -H "Authorization: Bearer {token}"
```

### 3. Close an Episode
```bash
curl -X PUT http://localhost:5000/api/Clinical/episodes/1/close \
  -H "Authorization: Bearer {token}"
```

## Quick Start - Consultations

### 1. Start a Consultation (from Appointment)
```bash
curl -X POST http://localhost:5000/api/Clinical/consultations/start \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "appointmentId": 1,
    "doctorId": 1,
    "episodeId": 1
  }'
```

**Response:**
```json
{
  "id": 1,
  "appointmentId": 1,
  "episodeId": 1,
  "patientId": 1,
  "doctorId": 1,
  "startedAt": "2024-01-20T14:30:00Z",
  "endedAt": null,
  "clinicalSummary": null,
  "patientName": "John Doe",
  "doctorName": "Dr. Smith",
  "appointmentStatus": "Completed",
  "episodeTitle": "Pregnancy 2024"
}
```

### 2. Get Active Consultation for Doctor
```bash
curl http://localhost:5000/api/Clinical/doctors/1/active-consultation \
  -H "Authorization: Bearer {token}"
```

### 3. End Consultation
```bash
curl -X PUT http://localhost:5000/api/Clinical/consultations/1/end \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "clinicalSummary": "Patient presented with regular prenatal checkup. All vitals normal. Next appointment scheduled for 4 weeks."
  }'
```

### 4. Get Patient Consultations
```bash
curl http://localhost:5000/api/Clinical/patients/1/consultations \
  -H "Authorization: Bearer {token}"
```

### 5. Link Consultation to Episode
```bash
curl -X PUT http://localhost:5000/api/Clinical/consultations/1/link-episode \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {token}" \
  -d '{
    "episodeId": 1
  }'
```

## Complete Workflow

### Typical Doctor Workflow

1. **Morning**: View active consultations
   ```bash
   GET /api/Clinical/doctors/{doctorId}/active-consultation
   ```

2. **Start Visit**: Click "Start Visit" on appointment
   ```bash
   POST /api/Clinical/consultations/start
   ```

3. **During Visit**: Record vitals, assessments, orders (existing features)

4. **End Visit**: Click "Finish"
   ```bash
   PUT /api/Clinical/consultations/{id}/end
   ```

5. **Link to Episode**: Group consultation under condition
   ```bash
   PUT /api/Clinical/consultations/{id}/link-episode
   ```

## Testing with Web UI

1. Navigate to `/episode-consultation-test.html`
2. Login with valid credentials
3. **Episodes Tab**:
   - Create new episodes for patients
   - View patient episodes
   - Close episodes
4. **Consultations Tab**:
   - Start consultations from appointments
   - View active consultations by doctor
   - End consultations with summaries
   - Link consultations to episodes
5. **History Tab**:
   - View consultation history
   - Filter by patient

## Common Scenarios

### Scenario 1: Pregnancy Care
```bash
# Create pregnancy episode
POST /api/Clinical/episodes
{
  "patientId": 5,
  "title": "Pregnancy 2024",
  "startDate": "2024-01-01"
}

# First prenatal visit
POST /api/Clinical/consultations/start
{
  "appointmentId": 10,
  "doctorId": 2,
  "episodeId": 3
}

# End with notes
PUT /api/Clinical/consultations/15/end
{
  "clinicalSummary": "Initial prenatal visit. Due date calculated. Prenatal vitamins prescribed."
}
```

### Scenario 2: Diabetes Management
```bash
# Create episode
POST /api/Clinical/episodes
{
  "patientId": 8,
  "title": "Diabetes Type 2 Management",
  "startDate": "2024-03-01"
}

# Multiple consultations linked to same episode
POST /api/Clinical/consultations/start
{
  "appointmentId": 25,
  "doctorId": 1,
  "episodeId": 7
}

PUT /api/Clinical/consultations/50/end
{
  "clinicalSummary": "Blood sugar levels improving with current medication."
}
```

### Scenario 3: Single Visit (No Episode)
```bash
# Start consultation without episode
POST /api/Clinical/consultations/start
{
  "appointmentId": 30,
  "doctorId": 3,
  "episodeId": null
}

# Later decide to link to episode
PUT /api/Clinical/consultations/60/link-episode
{
  "episodeId": 12
}
```

## Error Handling

### Active Consultation Already Exists
```json
{
  "error": {
    "code": "ACTIVE_CONSULTATION_EXISTS",
    "message": "Doctor already has an active consultation with ID 5"
  }
}
```

### Episode Not Found
```json
{
  "error": {
    "code": "NOT_FOUND",
    "message": "Episode with ID 999 not found"
  }
  }
}
```

## Important Notes
- Consultations must be started from appointments (requires Story 10-1)
- Doctors can only have one active consultation at a time
- Episodes are optional - consultations work independently
- Linking consultation to episode can be done before or after ending
- All timestamps are in UTC
- Clinical summary is required when ending consultation
- Patient and doctor names are included in consultation response
- Episode title is included in consultation response for dashboard display
