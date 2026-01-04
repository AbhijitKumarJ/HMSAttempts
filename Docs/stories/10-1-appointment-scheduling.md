# Story 10.1: Appointment Scheduling

**Status:** ready-for-dev

## Story

As a Receptionist,
I want to schedule an appointment for a patient,
So that they can see a doctor at a specific time.

## Acceptance Criteria

1.  **Usage:** User selects Patient, Doctor, Date, and Time.
2.  **Validation:** System checks for double-booking (Doctor cannot have two active appointments at the same time).
3.  **Persistence:** Saves to `sch_appointments`.
4.  **UI:** Calendar view showing Doctor's availability.

## Technical Implementation

### Database
*   **Table:** `sch_appointments` (patient_id, doctor_id, appointment_date, status).

### Backend
*   **Service:** `SchedulingService.BookAppointmentAsync`.
*   **Logic:** `WHERE doctor_id = X AND appointment_date = Y` check before INSERT.
