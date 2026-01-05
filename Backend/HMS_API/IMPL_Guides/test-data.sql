-- HMS Master/Test Data for API Testing
-- This script provides sample data for testing Patient Search and Appointment Scheduling APIs

-- Users (Doctors)
-- Note: These users will be used as doctors for appointments
-- Password hashes are placeholders - replace with actual bcrypt hashes

INSERT INTO users (username, password_hash, is_active, created_at)
VALUES 
    ('dr_smith', '$2a$11$XwF3hXzZL6j4QHlX1L4B3K5G0mN1jz5BmP6gVh1Iu0', true, NOW()),
    ('dr_jones', '$2a$11$XwF3hXzZL6j4QHlX1L4B3K5G0mN1jz5BmP6gVh1Iu0', true, NOW()),
    ('dr_wilson', '$2a$11$XwF3hXzZL6j4QHlX1L4B3K5G0mN1jz5BmP6gVh1Iu0', true, NOW()),
    ('receptionist1', '$2a$11$XwF3hXzZL6j4QHlX1L4B3K5G0mN1jz5BmP6gVh1Iu0', true, NOW())
ON CONFLICT (username) DO NOTHING;

-- ============================================
-- 2. PATIENTS
-- ============================================
-- Sample patients for testing search functionality

INSERT INTO pat_patients (mrn, first_name, last_name, gender, dob, contact_info, is_emergency_reg, created_at)
VALUES 
    ('MRN-2026-0001', 'John', 'Smith', 'M', '1980-05-15', 
     '{"email": "john.smith@email.com", "phone": "555-0101", "address": "123 Main St", "insuranceProvider": "BlueCross", "policyNumber": "POL-001"}', 
     false, NOW()),
    
    ('MRN-2026-0002', 'Jane', 'Doe', 'F', '1985-08-22',
     '{"email": "jane.doe@email.com", "phone": "555-0102", "address": "456 Oak Ave", "insuranceProvider": "Aetna", "policyNumber": "POL-002"}',
     false, NOW()),
    
    ('MRN-2026-0003', 'Robert', 'Johnson', 'M', '1972-11-30',
     '{"email": "robert.j@email.com", "phone": "555-0103", "address": "789 Pine Rd", "insuranceProvider": "UnitedHealth", "policyNumber": "POL-003"}',
     false, NOW()),
    
    ('MRN-2026-0004', 'Emily', 'Williams', 'F', '1990-03-10',
     '{"email": "emily.w@email.com", "phone": "555-0104", "address": "321 Elm St", "insuranceProvider": "Cigna", "policyNumber": "POL-004"}',
     false, NOW()),
    
    ('MRN-2026-0005', 'Michael', 'Brown', 'M', '1968-07-18',
     '{"email": "michael.b@email.com", "phone": "555-0105", "address": "654 Cedar Ln", "insuranceProvider": "BlueCross", "policyNumber": "POL-005"}',
     false, NOW()),
    
    ('MRN-2026-0006', 'Sarah', 'Davis', 'F', '1995-12-05',
     '{"email": "sarah.d@email.com", "phone": "555-0106", "address": "987 Birch Dr", "insuranceProvider": "Aetna", "policyNumber": "POL-006"}',
     false, NOW()),
    
    ('MRN-2026-0007', 'David', 'Miller', 'M', '1978-02-28',
     '{"email": "david.m@email.com", "phone": "555-0107", "address": "147 Maple Ct", "insuranceProvider": "Medicare", "policyNumber": "POL-007"}',
     false, NOW()),
    
    ('MRN-2026-0008', 'Lisa', 'Anderson', 'F', '1988-09-14',
     '{"email": "lisa.a@email.com", "phone": "555-0108", "address": "258 Spruce Way", "insuranceProvider": "BlueCross", "policyNumber": "POL-008"}',
     false, NOW()),
    
    ('MRN-2026-0009', 'James', 'Taylor', 'M', '1982-04-25',
     '{"email": "james.t@email.com", "phone": "555-0109", "address": "369 Oak Blvd", "insuranceProvider": "UnitedHealth", "policyNumber": "POL-009"}',
     false, NOW()),
    
    ('MRN-2026-0010', 'Jennifer', 'Thomas', 'F', '1992-06-08',
     '{"email": "jennifer.t@email.com", "phone": "555-0110", "address": "741 Pine Ave", "insuranceProvider": "Cigna", "policyNumber": "POL-010"}',
     false, NOW())
ON CONFLICT (mrn) DO NOTHING;

-- ============================================
-- 3. APPOINTMENTS
-- ============================================
-- Sample appointments for testing search by date range and doctor

-- Get today's date for reference
-- Note: In actual execution, replace these with actual dates

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0001'),
    (SELECT id FROM users WHERE username = 'dr_smith'),
    NOW() + INTERVAL '1 day',
    'Scheduled',
    'Annual checkup - routine physical examination',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0001')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_smith')
ON CONFLICT DO NOTHING;

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0002'),
    (SELECT id FROM users WHERE username = 'dr_smith'),
    NOW() + INTERVAL '1 day' + INTERVAL '2 hours',
    'Scheduled',
    'Follow-up appointment - blood pressure monitoring',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0002')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_smith')
ON CONFLICT DO NOTHING;

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0003'),
    (SELECT id FROM users WHERE username = 'dr_jones'),
    NOW() + INTERVAL '2 days',
    'Scheduled',
    'Specialist consultation - cardiology referral',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0003')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_jones')
ON CONFLICT DO NOTHING;

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0004'),
    (SELECT id FROM users WHERE username = 'dr_jones'),
    NOW() + INTERVAL '2 days' + INTERVAL '3 hours',
    'Scheduled',
    'MRI scan - knee injury follow-up',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0004')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_jones')
ON CONFLICT DO NOTHING;

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0005'),
    (SELECT id FROM users WHERE username = 'dr_wilson'),
    NOW() + INTERVAL '3 days',
    'Scheduled',
    'Pre-surgical consultation',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0005')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_wilson')
ON CONFLICT DO NOTHING;

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0006'),
    (SELECT id FROM users WHERE username = 'dr_wilson'),
    NOW() + INTERVAL '4 days',
    'Scheduled',
    'Post-surgery follow-up',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0006')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_wilson')
ON CONFLICT DO NOTHING;

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0007'),
    (SELECT id FROM users WHERE username = 'dr_smith'),
    NOW() + INTERVAL '5 days',
    'Scheduled',
    'Diabetes management consultation',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0007')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_smith')
ON CONFLICT DO NOTHING;

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0008'),
    (SELECT id FROM users WHERE username = 'dr_smith'),
    NOW() + INTERVAL '1 week',
    'Scheduled',
    'Vaccination appointment - flu shot',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0008')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_smith')
ON CONFLICT DO NOTHING;

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0009'),
    (SELECT id FROM users WHERE username = 'dr_jones'),
    NOW() + INTERVAL '1 week' + INTERVAL '2 days',
    'Scheduled',
    'Laboratory results review',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0009')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_jones')
ON CONFLICT DO NOTHING;

INSERT INTO sch_appointments (patient_id, doctor_id, appointment_date, status, reason_for_visit, created_at)
SELECT 
    (SELECT id FROM pat_patients WHERE mrn = 'MRN-2026-0010'),
    (SELECT id FROM users WHERE username = 'dr_wilson'),
    NOW() + INTERVAL '2 weeks',
    'Scheduled',
    'Physical therapy assessment',
    NOW()
WHERE EXISTS (SELECT 1 FROM pat_patients WHERE mrn = 'MRN-2026-0010')
  AND EXISTS (SELECT 1 FROM users WHERE username = 'dr_wilson')
ON CONFLICT DO NOTHING;

-- ============================================
-- VERIFICATION QUERIES
-- ============================================

-- Check patients created:
-- SELECT id, mrn, first_name, last_name FROM pat_patients ORDER BY created_at DESC LIMIT 10;

-- Check doctors (users) created:
-- SELECT id, username FROM users WHERE username LIKE 'dr_%';

-- Check doctor roles assigned:
-- SELECT u.id, u.username, r.name as role
-- FROM users u
-- JOIN user_roles ur ON u.id = ur.user_id
-- JOIN roles r ON ur.role_id = r.id
-- WHERE u.username LIKE 'dr_%';

-- Check appointments created:
-- SELECT a.id, p.mrn, p.first_name, p.last_name, u.username as doctor_name, a.appointment_date, a.status
-- FROM sch_appointments a
-- JOIN pat_patients p ON a.patient_id = p.id
-- JOIN users u ON a.doctor_id = u.id
-- ORDER BY a.appointment_date;

-- Test search appointments for specific date range:
-- SELECT * FROM sch_appointments 
-- WHERE appointment_date >= NOW() AND appointment_date <= NOW() + INTERVAL '1 day'
-- ORDER BY appointment_date;

-- Test search for specific doctor:
-- SELECT a.id, p.first_name, p.last_name, a.appointment_date
-- FROM sch_appointments a
-- JOIN pat_patients p ON a.patient_id = p.id
-- WHERE a.doctor_id = (SELECT id FROM users WHERE username = 'dr_smith')
-- ORDER BY a.appointment_date;
