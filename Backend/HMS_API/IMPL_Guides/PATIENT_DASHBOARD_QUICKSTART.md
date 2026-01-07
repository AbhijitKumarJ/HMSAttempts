# Patient Dashboard & Timeline Quick Start Guide

## Overview

The patient dashboard provides a consolidated view of patient history and current status, displaying a 3-column "cockpit" layout with pinned vitals ribbon and chronological timeline of events (visits, labs, appointments).

## Story Reference

- **Story**: `Docs/stories/4-2-patient-dashboard-timeline.md`
- **Status**: Implemented

## API Endpoint

### GET `/api/patients/{mrn}/summary`

Retrieve patient dashboard data including demographics, latest vitals, allergies, active problems, and timeline events.

**URL Parameters:**
- `mrn` (string) - Patient Medical Record Number

**Response (200 OK):**
```json
{
  "message": "Patient summary for MRN MRN-2026-0001.",
  "data": {
    "demographics": {
      "id": 1,
      "mrn": "MRN-2026-0001",
      "firstName": "John",
      "lastName": "Doe",
      "gender": "M",
      "dob": "1990-05-15",
      "age": "35 years",
      "hasAllergies": false
    },
    "vitalsRibbon": {
      "recordedAt": "2026-01-07T10:00:00Z",
      "bpSystolic": 120,
      "bpDiastolic": 80,
      "heartRate": 72,
      "temperature": 37.0,
      "spo2": 98
    },
    "allergies": [],
    "activeProblems": [],
    "recentTimelineEvents": [
      {
        "type": "Consultation",
        "eventDate": "2026-01-07T09:30:00Z",
        "summary": "Consultation with doctor1",
        "details": "Patient presented with mild fever and cough...",
        "performedBy": "doctor1"
      },
      {
        "type": "LabResult",
        "eventDate": "2026-01-06T14:00:00Z",
        "summary": "Complete Blood Count",
        "details": "WBC: 7.5, RBC: 4.8, HGB: 14.5",
        "performedBy": "doctor1"
      },
      {
        "type": "Appointment",
        "eventDate": "2026-01-05T10:00:00Z",
        "summary": "General Checkup",
        "details": "Status: completed",
        "performedBy": "doctor1"
      }
    ]
  }
}
```

**Error Response (404 Not Found):**
```json
{
  "error": {
    "code": "PATIENT_NOT_FOUND",
    "message": "Patient with MRN MRN-XXXX-XXXX not found"
  }
}
```

## Data Structure

### PatientSummaryDto

| Property | Type | Description |
|----------|-------|-------------|
| demographics | DemographicsDto | Patient basic information |
| vitalsRibbon | VitalsRibbonDto | Latest vital signs |
| allergies | AllergyDto[] | Known allergies (currently empty) |
| activeProblems | ActiveProblemDto[] | Active medical conditions (currently empty) |
| recentTimelineEvents | TimelineEventDto[] | Chronological patient events |

### DemographicsDto

| Property | Type | Description |
|----------|-------|-------------|
| id | int | Internal patient ID |
| mrn | string | Medical Record Number |
| firstName | string | Patient's first name |
| lastName | string | Patient's last name |
| gender | string? | Gender (M/F/O/U) |
| dob | DateOnly? | Date of birth |
| age | string? | Calculated age |
| hasAllergies | bool | Whether patient has allergies |

### VitalsRibbonDto

| Property | Type | Description |
|----------|-------|-------------|
| recordedAt | DateTime? | When vitals were recorded |
| bpSystolic | int? | Blood pressure systolic (mmHg) |
| bpDiastolic | int? | Blood pressure diastolic (mmHg) |
| heartRate | int? | Heart rate (bpm) |
| temperature | decimal? | Body temperature (°C) |
| spo2 | int? | Oxygen saturation (%) |

### TimelineEventDto

| Property | Type | Description |
|----------|-------|-------------|
| type | string | Event type: "Consultation", "Appointment", "LabResult" |
| eventDate | DateTime | When the event occurred |
| summary | string? | Brief description |
| details | string? | Detailed notes |
| performedBy | string? | Staff member who performed the action |

## Usage Flow

### Step 1: Authenticate

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -c cookies.txt \
  -d '{
    "username": "doctor1",
    "password": "password123"
  }'
```

Extract access token:
```bash
export ACCESS_TOKEN="your-jwt-token-here"
```

### Step 2: Get Patient Summary

```bash
MRN="MRN-2026-0001"

curl -X GET http://localhost:5000/api/patients/$MRN/summary \
  -H "Authorization: Bearer $ACCESS_TOKEN"
```

Expected response with all dashboard data.

### Step 3: Decode and Use Response

The response includes:
1. **Demographics** - For patient identification in header
2. **Vitals Ribbon** - For always-visible vital signs display
3. **Timeline Events** - For chronological history view
4. **Allergies/Problems** - For clinical decision support

## Frontend Integration

### Angular Service Example

```typescript
// patient.service.ts
export interface PatientSummary {
  demographics: Demographics;
  vitalsRibbon: VitalsRibbon;
  allergies: Allergy[];
  activeProblems: ActiveProblem[];
  recentTimelineEvents: TimelineEvent[];
}

export interface Demographics {
  id: number;
  mrn: string;
  firstName: string;
  lastName: string;
  gender: string | null;
  dob: string | null;
  age: string | null;
  hasAllergies: boolean;
}

export interface VitalsRibbon {
  recordedAt: string | null;
  bpSystolic: number | null;
  bpDiastolic: number | null;
  heartRate: number | null;
  temperature: number | null;
  spo2: number | null;
}

export interface TimelineEvent {
  type: string;
  eventDate: string;
  summary: string | null;
  details: string | null;
  performedBy: string | null;
}

getPatientSummary(mrn: string): Observable<PatientSummary> {
  return this.http.get<any>(
    `${this.apiUrl}/patients/${mrn}/summary`,
    {
      headers: this.getAuthHeaders()
    }
  ).pipe(
    map(response => response.data)
  );
}
```

### Angular Dashboard Component

```typescript
// patient-dashboard.component.ts
@Component({
  selector: 'app-patient-dashboard',
  template: `
    <div class="dashboard-layout">
      <!-- Left Column: Queue -->
      <aside class="queue-column">
        <h3>Patient Queue</h3>
        @for (patient of queuedPatients; track patient.mrn) {
          <div class="patient-card" (click)="selectPatient(patient)">
            {{ patient.firstName }} {{ patient.lastName }}
            <span class="mrn">{{ patient.mrn }}</span>
          </div>
        }
      </aside>

      <!-- Center Column: Vitals + Timeline -->
      <main class="main-column">
        <!-- Pinned Vitals Ribbon -->
        @if (summary) {
          <div class="vitals-ribbon">
            <div class="ribbon-header">
              {{ summary.demographics.firstName }} {{ summary.demographics.lastName }}
              ({{ summary.demographics.mrn }})
            </div>
            @if (summary.demographics.hasAllergies) {
              <div class="allergy-alert">
                ⚠️ Has Allergies
              </div>
            }
            <div class="vitals-grid">
              <div class="vital-item">
                <span class="label">BP</span>
                <span class="value">
                  {{ summary.vitalsRibbon.bpSystolic || '--' }}/
                  {{ summary.vitalsRibbon.bpDiastolic || '--' }}
                </span>
              </div>
              <div class="vital-item">
                <span class="label">HR</span>
                <span class="value">
                  {{ summary.vitalsRibbon.heartRate || '--' }} bpm
                </span>
              </div>
              <div class="vital-item">
                <span class="label">Temp</span>
                <span class="value">
                  {{ summary.vitalsRibbon.temperature || '--' }} °C
                </span>
              </div>
              <div class="vital-item">
                <span class="label">SpO2</span>
                <span class="value">
                  {{ summary.vitalsRibbon.spo2 || '--' }}%
                </span>
              </div>
            </div>
            <div class="recorded-at">
              Recorded: {{ summary.vitalsRibbon.recordedAt | date:'medium' }}
            </div>
          </div>
        }

        <!-- Chronological Timeline -->
        <div class="timeline">
          <h3>Patient Timeline</h3>
          @for (event of summary?.recentTimelineEvents; track event) {
            <div class="timeline-event event-{{ event.type.toLowerCase() }}">
              <div class="event-icon">
                @switch (event.type) {
                  @case ('Consultation') { 🩺 }
                  @case ('LabResult') { 🧪 }
                  @case ('Appointment') { 📅 }
                }
              </div>
              <div class="event-content">
                <div class="event-header">
                  <span class="event-type">{{ event.type }}</span>
                  <span class="event-date">{{ event.eventDate | date:'short' }}</span>
                </div>
                <div class="event-summary">{{ event.summary }}</div>
                @if (event.details) {
                  <div class="event-details">{{ event.details }}</div>
                }
                <div class="event-performer">
                  By: {{ event.performedBy || 'Unknown' }}
                </div>
              </div>
            </div>
          }
        </div>
      </main>

      <!-- Right Column: Actions -->
      <aside class="actions-column">
        <h3>Actions</h3>
        <button class="action-btn">Add Note</button>
        <button class="action-btn">New Order</button>
        <button class="action-btn">Schedule Follow-up</button>
      </aside>
    </div>
  `
})
export class PatientDashboardComponent {
  summary: PatientSummary | null = null;
  
  constructor(
    private patientService: PatientService,
    private route: ActivatedRoute
  ) {}
  
  ngOnInit() {
    const mrn = this.route.snapshot.paramMap.get('mrn');
    if (mrn) {
      this.loadPatientSummary(mrn);
    }
  }
  
  loadPatientSummary(mrn: string) {
    this.patientService.getPatientSummary(mrn).subscribe({
      next: (data) => {
        this.summary = data;
      },
      error: (err) => {
        console.error('Failed to load patient summary:', err);
      }
    });
  }
}
```

### CSS Layout (3-Column Cockpit)

```css
/* 3-Column Layout */
.dashboard-layout {
  display: grid;
  grid-template-columns: 250px 1fr 300px;
  gap: 20px;
  height: 100vh;
}

/* Left Column - Queue */
.queue-column {
  background: #f8f9fa;
  padding: 20px;
  overflow-y: auto;
}

/* Center Column - Vitals + Timeline */
.main-column {
  display: flex;
  flex-direction: column;
  gap: 20px;
  overflow-y: auto;
  padding: 20px;
}

/* Vitals Ribbon */
.vitals-ribbon {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}

.vitals-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 15px;
  margin: 15px 0;
}

.vital-item {
  text-align: center;
}

.vital-item .label {
  font-size: 12px;
  opacity: 0.9;
}

.vital-item .value {
  font-size: 24px;
  font-weight: bold;
}

/* Timeline */
.timeline {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.timeline-event {
  display: flex;
  gap: 15px;
  padding: 15px;
  border-bottom: 1px solid #e0e0e0;
}

.event-consultation { border-left: 4px solid #667eea; }
.event-labresult { border-left: 4px solid #f093fb; }
.event-appointment { border-left: 4px solid #4facfe; }

/* Right Column - Actions */
.actions-column {
  background: #fff;
  padding: 20px;
  border-left: 1px solid #e0e0e0;
}

.action-btn {
  width: 100%;
  padding: 12px;
  margin-bottom: 10px;
  background: #4facfe;
  color: white;
  border: none;
  border-radius: 6px;
  cursor: pointer;
}
```

## Performance Considerations

### Database Query Optimization

The API uses EF Core projection to avoid over-fetching:

```csharp
// Efficient single query with projections
public async Task<...> GetPatientSummaryDataAsync(string mrn)
{
    var patient = await _context.PatPatients
        .FirstOrDefaultAsync(p => p.Mrn == mrn);

    var latestVital = await _context.ClinVitals
        .Where(v => v.PatientId == patient.Id)
        .OrderByDescending(v => v.RecordedAt)
        .FirstOrDefaultAsync();

    var consultations = await _context.ClinConsultations
        .Where(c => c.PatientId == patient.Id && c.StartedAt.HasValue)
        .OrderByDescending(c => c.StartedAt)
        .Take(10)  // Limit to 10 most recent
        .ToListAsync();
    
    // Similar for appointments and labs...
}
```

### Frontend Performance

1. **Load Critical Data First**: Ribbon + Recent Timeline (< 2s)
2. **Lazy Load Details**: Additional timeline events on scroll
3. **Use rxResource**: Angular 17+ resource for reactive data loading
4. **Memoize Calculations**: Age calculation, etc.

```typescript
// Angular 17+ resource
patientSummary = rxResource(
  toSignal(this.route.paramMap),
  (params) => ({
    source: this.patientService.getPatientSummary(params.get('mrn')!),
    request: params
  })
);
```

## Database Setup

### Seed Test Patient Data

```sql
-- Create test patient
INSERT INTO pat_patients (mrn, first_name, last_name, gender, dob, created_at)
VALUES ('MRN-2026-0001', 'John', 'Doe', 'M', '1990-05-15', NOW());

-- Get patient ID
\set patientId (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0001')

-- Create test doctor
INSERT INTO users (username, password_hash, is_active) VALUES
('doctor1', '$2a$11$abcdefghijklmnopqrstuvw', TRUE);

INSERT INTO roles (name) VALUES ('Doctor');
INSERT INTO user_roles (user_id, role_id)
VALUES ((SELECT id FROM users WHERE username = 'doctor1'), (SELECT id FROM roles WHERE name = 'Doctor'));

-- Add latest vitals
INSERT INTO clin_vitals (patient_id, bp_systolic, bp_diastolic, heart_rate, temperature, spo2, recorded_at, recorded_by)
VALUES (:patientId, 120, 80, 72, 37.0, 98, NOW(), (SELECT id FROM users WHERE username = 'doctor1'));

-- Create consultation
INSERT INTO clin_consultations (patient_id, doctor_id, started_at, clinical_summary)
VALUES (:patientId, (SELECT id FROM users WHERE username = 'doctor1'), NOW() - INTERVAL '1 day', 'Patient presented with mild fever and cough. Prescribed rest and hydration.');

-- Create appointment
INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
VALUES (:patientId, (SELECT id FROM users WHERE username = 'doctor1'), NOW() - INTERVAL '2 days', 'completed', 'General Checkup', NOW() - INTERVAL '3 days');

-- Create order and lab result
INSERT INTO ord_orders (patient_id, ordered_by, type, description, status, created_at)
VALUES (:patientId, (SELECT id FROM users WHERE username = 'doctor1'), 'Lab', 'Complete Blood Count', 'completed', NOW() - INTERVAL '2 days');

INSERT INTO lab_results (order_id, result_summary, result_data, released_at)
VALUES ((SELECT id FROM ord_orders WHERE patient_id = :patientId ORDER BY id DESC LIMIT 1), 'Complete Blood Count', '{"WBC": 7.5, "RBC": 4.8, "HGB": 14.5, "HCT": 42.0, "PLT": 250}', NOW() - INTERVAL '2 days');
```

## Common Issues

### 1. Patient not found (404)

**Cause**: Invalid MRN or patient doesn't exist

**Solution**:
```bash
# List existing patients
psql -U postgres -d hms -c "SELECT mrn, first_name, last_name FROM pat_patients LIMIT 10;"

# Use correct MRN
```

### 2. No vitals in response

**Cause**: No vitals recorded for patient

**Solution**:
```bash
# Add vitals
psql -U postgres -d hms -c "
  INSERT INTO clin_vitals (patient_id, bp_systolic, bp_diastolic, heart_rate, temperature, spo2, recorded_at, recorded_by)
  VALUES (
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0001'),
    120, 80, 72, 37.0, 98, NOW(),
    (SELECT id FROM users WHERE username = 'doctor1')
  );
"
```

### 3. Empty timeline

**Cause**: No consultations, appointments, or labs for patient

**Solution**:
```bash
# Create sample events (see Database Setup section above)
```

### 4. Slow response time

**Cause**: Inefficient queries or missing indexes

**Solution**:
```sql
-- Add indexes for performance
CREATE INDEX idx_vitals_patient_recorded ON clin_vitals(patient_id, recorded_at DESC);
CREATE INDEX idx_consultations_patient_started ON clin_consultations(patient_id, started_at DESC);
CREATE INDEX idx_appointments_patient_date ON sch_appointments(patient_id, appointment_date DESC);
CREATE INDEX idx_lab_results_order_released ON lab_results(order_id, released_at DESC);
```

### 5. Age calculation wrong

**Cause**: DOB format or timezone issue

**Solution**:
```bash
# Verify DOB format
psql -U postgres -d hms -c "SELECT mrn, dob, EXTRACT(YEAR FROM AGE(dob)) as age FROM pat_patients WHERE mrn = 'MRN-2026-0001';"
```

## Testing Checklist

Before deploying to production:

- ✅ Patient summary returns demographics correctly
- ✅ Latest vitals are accurate
- ✅ Timeline is chronologically ordered
- ✅ Multiple event types display correctly
- ✅ Age is calculated correctly
- ✅ Allergy flag is accurate
- ✅ Response time < 2 seconds for critical data
- ✅ Invalid MRN returns 404
- ✅ Unauthenticated requests are rejected

## UI Components

### Vitals Ribbon
- Always visible (pinned)
- Shows BP, HR, Temperature, SpO2
- Displays recording timestamp
- Shows allergy alert if applicable

### Timeline
- Scrollable list
- Mixed event types (Consultation, Lab, Appointment)
- Chronological order (newest first)
- Visual distinction by event type
- Shows performer and details

### Action Area
- Context-aware actions
- Add clinical notes
- Create orders
- Schedule follow-ups

## Next Steps

After patient dashboard is working:

1. **Add Allergy Management** - CRUD operations for allergies
2. **Add Problem List** - Active conditions tracking
3. **Implement Pagination** - Infinite scroll for timeline
4. **Add Filtering** - Filter timeline by event type
5. **Add Export** - PDF export of patient summary
6. **Add Offline Support** - Cache dashboard data
7. **Add Real-time Updates** - SignalR for new events

## Resources

- **Story Details**: `Docs/stories/4-2-patient-dashboard-timeline.md`
- **Backend Code**: `HMS.Business/Patient/PatientService.cs`
- **Repository**: `HMS.Data/Patient/PatientRepository.cs`
- **Controller**: `HMS.API/Controllers/Patient/PatientController.cs`
- **Entities**: `HMS.Entity/Patient/PatientSummaryDto.cs`
- **Test Client**: `wwwroot/patient-dashboard.html`

## Support

If you encounter issues:

1. Check logs in console output
2. Verify patient exists in database
3. Ensure JWT token is valid
4. Check vitals and timeline tables have data
5. Verify EF Core queries are efficient

The patient dashboard module is now ready for integration with the clinical cockpit!