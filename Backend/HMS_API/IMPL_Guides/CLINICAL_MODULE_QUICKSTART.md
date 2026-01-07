# Clinical Module Quickstart Guide

## Prerequisites
- HMS.API running on `http://localhost:5000`
- PostgreSQL database configured and running
- Existing patient records in database

## Quick Start - Vitals Capture

### 1. Validate Vitals Data
```bash
curl -X POST http://localhost:5000/api/Clinical/vitals/validate \
  -H "Content-Type: application/json" \
  -d '{
    "patientId": 1,
    "bpSystolic": 145,
    "bpDiastolic": 95,
    "heartRate": 105,
    "temperature": 38.5,
    "spo2": 92
  }'
```

**Response:**
```json
{
  "isValid": false,
  "alerts": [
    {
      "field": "BpSystolic",
      "alertLevel": "Warning",
      "message": "Systolic BP elevated - Hypertension Stage 1"
    },
    {
      "field": "HeartRate",
      "alertLevel": "Warning",
      "message": "Heart rate elevated"
    },
    {
      "field": "Temperature",
      "alertLevel": "Warning",
      "message": "Temperature elevated - Fever"
    },
    {
      "field": "Spo2",
      "alertLevel": "Warning",
      "message": "SpO2 below 94% - Mild Hypoxemia"
    }
  ],
  "overallPriority": "Medium"
}
```

### 2. Record Vitals
```bash
curl -X POST http://localhost:5000/api/Clinical/vitals \
  -H "Content-Type: application/json" \
  -d '{
    "patientId": 1,
    "consultationId": null,
    "bpSystolic": 120,
    "bpDiastolic": 80,
    "heartRate": 72,
    "temperature": 36.5,
    "spo2": 98
  }'
```

### 3. Get Patient Vitals History
```bash
curl http://localhost:5000/api/Clinical/patients/1/vitals
```

## Quick Start - Form Templates

### 1. Create a Triage Form Template
```bash
curl -X POST http://localhost:5000/api/Clinical/templates \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Triage Assessment",
    "isActive": true,
    "version": 1,
    "fields": [
      {
        "fieldId": 1,
        "labelOverride": "Chief Complaint",
        "displayOrder": 1,
        "isRequired": true,
        "uiControl": "input"
      },
      {
        "fieldId": 2,
        "labelOverride": "Onset Time",
        "displayOrder": 2,
        "isRequired": true,
        "uiControl": "datetime"
      }
    ]
  }'
```

### 2. List All Templates
```bash
curl http://localhost:5000/api/Clinical/templates
```

### 3. Get Specific Template
```bash
curl http://localhost:5000/api/Clinical/templates/1
```

### 4. Get Alert Rules
```bash
curl http://localhost:5000/api/Clinical/vitals/alert-rules
```

## Testing with Web UI

1. Navigate to `/clinical-test.html`
2. Use the Vitals Capture form to record patient vitals
3. Use the Validation tab to test alert rules
4. Use the Form Builder tab to create templates
5. View validation results with color-coded alerts

## Common Alert Levels

- **Normal** - All values within healthy range
- **Warning** - Values slightly outside normal range (amber border)
- **Critical** - Values requiring immediate attention (red border)

## Important Notes
- Vitals must have a valid `patientId`
- `consultationId` is optional for standalone vital capture
- All validation happens on the server
- Alert rules are configurable via code constants in `ClinicalService.cs`
