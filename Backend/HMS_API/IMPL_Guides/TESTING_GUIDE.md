# HMS API Testing Guide

## Master/Test Data for New APIs

This guide provides sample data for testing the Patient Search and Appointment Scheduling APIs.

---

## 1. SQL Script

Run `test-data.sql` to populate your database with sample data:

```bash
psql -U your_username -d your_database -f test-data.sql
```

Or execute the SQL in your preferred PostgreSQL client.

---

## 2. Sample Data Overview

### Users (Doctors)
| Username      | Role       | Description |
|---------------|-------------|-------------|
| dr_smith      | Doctor      | Primary doctor for Smith clinic |
| dr_jones      | Doctor      | Cardiologist |
| dr_wilson     | Doctor      | Surgeon |
| receptionist1 | Staff       | Front desk receptionist |

### Patients (10 Records)
| MRN           | First Name | Last Name | Gender | DOB       | Email                  | Insurance       |
|----------------|-------------|-------------|----------|-----------|------------------------|-----------------|
| MRN-2026-0001 | John        | Smith       | M        | 1980-05-15 | john.smith@email.com | BlueCross        |
| MRN-2026-0002 | Jane        | Doe         | F        | 1985-08-22 | jane.doe@email.com   | Aetna           |
| MRN-2026-0003 | Robert      | Johnson     | M        | 1972-11-30 | robert.j@email.com   | UnitedHealth     |
| MRN-2026-0004 | Emily       | Williams    | F        | 1990-03-10 | emily.w@email.com   | Cigna           |
| MRN-2026-0005 | Michael     | Brown       | M        | 1968-07-18 | michael.b@email.com  | BlueCross        |
| MRN-2026-0006 | Sarah       | Davis       | F        | 1995-12-05 | sarah.d@email.com   | Aetna           |
| MRN-2026-0007 | David       | Miller      | M        | 1978-02-28 | david.m@email.com   | Medicare         |
| MRN-2026-0008 | Lisa        | Anderson    | F        | 1988-09-14 | lisa.a@email.com   | BlueCross        |
| MRN-2026-0009 | James       | Taylor      | M        | 1982-04-25 | james.t@email.com   | UnitedHealth     |
| MRN-2026-0010 | Jennifer    | Thomas      | F        | 1992-06-08 | jennifer.t@email.com| Cigna           |

### Appointments (10 Records)
| Patient MRN    | Doctor      | Date           | Reason                          |
|-----------------|-------------|----------------|---------------------------------|
| MRN-2026-0001  | dr_smith    | Today + 1 day   | Annual checkup - routine physical    |
| MRN-2026-0002  | dr_smith    | Today + 1 day   | Follow-up - blood pressure monitoring |
| MRN-2026-0003  | dr_jones    | Today + 2 days  | Cardiology referral                |
| MRN-2026-0004  | dr_jones    | Today + 2 days  | MRI scan - knee injury           |
| MRN-2026-0005  | dr_wilson   | Today + 3 days  | Pre-surgical consultation         |
| MRN-2026-0006  | dr_wilson   | Today + 4 days  | Post-surgery follow-up           |
| MRN-2026-0007  | dr_smith    | Today + 5 days  | Diabetes management              |
| MRN-2026-0008  | dr_smith    | Today + 1 week | Flu shot vaccination            |
| MRN-2026-0009  | dr_jones    | Today + 1 week + 2 days | Lab results review      |
| MRN-2026-0010  | dr_wilson   | Today + 2 weeks | Physical therapy assessment    |

---

## 3. API Testing Scenarios

### Doctors API

#### Test 1: Get All Doctors
```
GET /api/auth/doctors
```
**Expected:** Returns all 3 doctors

#### Test 2: Search Doctors by Name
```
GET /api/auth/doctors?q=smith
```
**Expected:** Returns dr_smith

#### Test 3: Search Doctors by ID
```
GET /api/auth/doctors?q=1
```
**Expected:** Returns doctor with ID 1

#### Test 4: No Results
```
GET /api/auth/doctors?q=xyzxyz
```
**Expected:** Returns empty array

### Patient Search API

#### Test 1: Search by First Name
```
GET /api/patients/search?q=John
```
**Expected:** Returns John Smith (MRN-2026-0001)

#### Test 2: Search by Last Name
```
GET /api/patients/search?q=Doe
```
**Expected:** Returns Jane Doe (MRN-2026-0002)

#### Test 3: Search by MRN
```
GET /api/patients/search?q=MRN-2026-0001
```
**Expected:** Returns John Smith

#### Test 4: Fuzzy Search
```
GET /api/patients/search?q=smit
```
**Expected:** Returns John Smith

#### Test 5: No Results
```
GET /api/patients/search?q=xyzxyz
```
**Expected:** Returns empty array

#### Test 6: Get All Patients
```
GET /api/patients
```
**Expected:** Returns all 10 patients

---

### Appointment Scheduling API

#### Test 1: Book New Appointment
```
POST /api/appointments
{
  "patientId": 1,
  "doctorId": 1,
  "appointmentDate": "2026-01-15T10:00:00Z",
  "reasonForVisit": "Test appointment"
}
```
**Expected:** Returns created appointment with ID

#### Test 2: Double-Booking Prevention (Same Doctor, Same Time)
```
POST /api/appointments
{
  "patientId": 1,
  "doctorId": 1,
  "appointmentDate": "2026-01-16T10:00:00Z",
  "reasonForVisit": "First appointment"
}

POST /api/appointments
{
  "patientId": 2,
  "doctorId": 1,
  "appointmentDate": "2026-01-16T10:00:00Z",
  "reasonForVisit": "Second appointment"
}
```
**Expected:** First succeeds, second returns 409 Conflict with double-booking error

#### Test 3: Search Appointments - Single Day
```
GET /api/appointments?startDate=2026-01-07
```
**Expected:** Returns all appointments for January 7, 2026

#### Test 4: Search Appointments - Date Range
```
GET /api/appointments?startDate=2026-01-07&endDate=2026-01-09
```
**Expected:** Returns appointments from Jan 7-9, 2026

#### Test 5: Search Appointments - Specific Doctor
```
GET /api/appointments?startDate=2026-01-07&doctorId=1
```
**Expected:** Returns only dr_smith's appointments

#### Test 6: Search Appointments - Date Range + Doctor
```
GET /api/appointments?startDate=2026-01-07&endDate=2026-01-10&doctorId=2
```
**Expected:** Returns dr_jones's appointments between Jan 7-10

#### Test 7: Search Appointments - All Doctors (No doctorId)
```
GET /api/appointments?startDate=2026-01-07&endDate=2026-01-14
```
**Expected:** Returns all appointments across all doctors for the date range

#### Test 8: Get Appointment by ID
```
GET /api/appointments/1
```
**Expected:** Returns full appointment details with patient and doctor names

---

## 4. UI Testing

### Patient Portal (patient-portal.html)
1. Login with `receptionist1` / `password123`
2. Test Emergency Registration:
   - Enter first name, last name, gender
   - Click Emergency Register
   - Note the MRN returned
3. Test Update Patient:
   - Use the MRN from registration
   - Add DOB, email, phone, address
   - Click Update Patient
4. Test Search by MRN:
   - Enter a known MRN
   - Click Get Patient
5. Test Patient Search:
   - Type "John" in search
   - Click Search Patients
   - Verify John Smith appears
6. Test Get All Patients:
   - Click Load All Patients
   - Verify all 10 patients returned

### Scheduling Test (scheduling-test.html)
1. Login with `receptionist1` / `password123`
2. Test Patient Search in Booking Form:
   - Type "Smith" in patient search
   - Select John Smith from dropdown
   - Verify patient ID is populated
3. Test Doctor Search:
   - Type "smith" in doctor search
   - Select dr_smith from dropdown
   - Verify doctor ID is populated
4. Test Book Appointment:
   - Select patient and doctor from dropdowns
   - Enter appointment date/time
   - Enter reason for visit
   - Click Book Appointment
   - Note the appointment ID returned
5. Test Search Appointments - Today:
   - Start date is auto-filled with today
   - Leave end date empty
   - Leave doctor ID empty
   - Click Search Appointments
   - Verify today's appointments for all doctors
6. Test Search Appointments - Date Range:
   - Set start date: 2026-01-07
   - Set end date: 2026-01-10
   - Click Search
   - Verify appointments across date range
7. Test Search Appointments - Doctor Filter:
   - Set start date: 2026-01-07
   - Set doctor ID: 1 (dr_smith)
   - Click Search
   - Verify only dr_smith's appointments
8. Test Search Appointments - Date Range + Doctor:
   - Set start date: 2026-01-07
   - Set end date: 2026-01-10
   - Set doctor ID: 2 (dr_jones)
   - Click Search
   - Verify dr_jones's appointments between Jan 7-10
9. Test Get Appointment by ID:
   - Use appointment ID from step 4
   - Click Get Appointment
   - Verify full details returned
10. Test Double-Booking Prevention:
   - Book appointment for Dr. Smith at 2026-01-20 10:00
   - Try to book another patient for Dr. Smith at same time
   - Verify second booking fails with error

---

## 5. Quick Test Commands (curl)

```bash
# Get all doctors
curl -H "Authorization: Bearer YOUR_TOKEN" \
  "http://localhost:5000/api/auth/doctors"

# Search doctors
curl -H "Authorization: Bearer YOUR_TOKEN" \
  "http://localhost:5000/api/auth/doctors?q=smith"

# Search patients
curl -H "Authorization: Bearer YOUR_TOKEN" \
  "http://localhost:5000/api/patients/search?q=Smith"

# Get all patients
curl -H "Authorization: Bearer YOUR_TOKEN" \
  "http://localhost:5000/api/patients"

# Book appointment
curl -X POST \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"patientId":1,"doctorId":1,"appointmentDate":"2026-01-20T10:00:00Z","reasonForVisit":"Test"}' \
  "http://localhost:5000/api/appointments"

# Search appointments (today)
curl -H "Authorization: Bearer YOUR_TOKEN" \
  "http://localhost:5000/api/appointments?startDate=2026-01-07"

# Search appointments (date range + doctor)
curl -H "Authorization: Bearer YOUR_TOKEN" \
  "http://localhost:5000/api/appointments?startDate=2026-01-07&endDate=2026-01-10&doctorId=1"
```

---

## 6. Notes

- **Password Hash**: The SQL uses placeholder `'$2a$11$...'` - replace with actual bcrypt hashes from your auth system
- **Dates**: Appointments use relative dates (`NOW() + INTERVAL '1 day'`) - adjust as needed
- **Conflict Resolution**: `ON CONFLICT DO NOTHING` prevents duplicate inserts - safe to run multiple times
- **Dependencies**: Ensure roles table has a "Doctor" role before running user_roles inserts
