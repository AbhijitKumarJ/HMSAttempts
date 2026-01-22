
import { Injectable, signal } from '@angular/core';

export interface Patient {
  mrn: string;
  firstName: string;
  lastName: string;
  dob: string;
  gender: string;
  phone: string;
  lastVisit: string;
  status: 'Checked In' | 'Waiting' | 'In Progress' | 'Discharged' | 'Pending Triage' | 'Ready for Exam' | 'Results Pending';
  insurance: string;
  balance: number;
  acuity?: 'Low' | 'Medium' | 'High' | 'Critical';
  allergies?: string[];
  // Clinical Context for Doctor
  chiefComplaint?: string;
  vitalsSummary?: string; // e.g., "150/90 • 98bpm"
  activeProblems?: string[];
  currentMeds?: string[];
  timeline?: TimelineEvent[];
}

export interface TimelineEvent {
  id: string;
  type: 'Note' | 'Lab' | 'Order' | 'Vitals';
  title: string;
  timestamp: string;
  author: string;
  details: string;
  status?: 'Pending' | 'Completed' | 'Final';
}

export interface Doctor {
  id: string;
  name: string;
  specialty: string;
  status: 'Available' | 'Busy' | 'Away';
  currentPatient?: string;
  nextFreeSlot: string;
}

export interface MedicationTask {
  id: string;
  patientMrn: string;
  patientName: string;
  drugName: string;
  dose: string;
  dueTime: string;
  status: 'Due' | 'Overdue' | 'Done';
}

export interface VitalAlert {
  patientMrn: string;
  patientName: string;
  room: string;
  alert: string; // e.g., "BP: 180/110"
  severity: 'High' | 'Critical';
}

export interface ResultInboxItem {
  id: string;
  patientName: string;
  patientMrn: string;
  testName: string;
  value: string;
  flag: 'High' | 'Low' | 'Critical';
  timestamp: string;
}

// --- Admin Interfaces ---
export interface SystemUser {
  id: string;
  name: string;
  username: string;
  roles: string[];
  status: 'Active' | 'Locked' | 'Inactive';
  lastLogin: string;
}

export interface AuditLog {
  id: string;
  timestamp: string;
  user: string;
  action: string;
  entity: string;
  details: string;
  diff?: { old: any; new: any };
}

export interface FormSchema {
  id: string;
  name: string;
  version: number;
  lastUpdated: string;
  schema: string; // JSON string
}

@Injectable({ providedIn: 'root' })
export class DataService {
  // Mock Patients
  private _patients: Patient[] = [
    { 
      mrn: 'MRN-8821', firstName: 'John', lastName: 'Doe', dob: '1985-04-12', gender: 'M', phone: '(555) 123-4567', lastVisit: '2023-11-10', status: 'Checked In', insurance: 'BlueCross', balance: 25.00, acuity: 'Low', allergies: ['Penicillin'],
      chiefComplaint: 'Sore throat', vitalsSummary: '120/80 • 72bpm', activeProblems: ['Seasonal Allergies'], currentMeds: ['Claritin 10mg'],
      timeline: [
        { id: 'ev-1', type: 'Note', title: 'Triage Note', timestamp: 'Today, 09:15 AM', author: 'Nurse Alex', details: 'Patient reports sore throat x2 days. No fever.' }
      ]
    },
    { 
      mrn: 'MRN-9932', firstName: 'Sarah', lastName: 'Connor', dob: '1992-08-23', gender: 'F', phone: '(555) 987-6543', lastVisit: '2024-01-15', status: 'Ready for Exam', insurance: 'Aetna', balance: 0.00, acuity: 'Medium', allergies: [],
      chiefComplaint: 'Abdominal Pain', vitalsSummary: '135/85 • 88bpm', activeProblems: ['GERD'], currentMeds: ['Omeprazole 20mg'],
      timeline: [
        { id: 'ev-2', type: 'Note', title: 'Triage Note', timestamp: 'Today, 09:30 AM', author: 'Nurse Alex', details: 'RLQ pain rated 6/10. Nausea present.' },
        { id: 'ev-3', type: 'Vitals', title: 'Vitals Recorded', timestamp: 'Today, 09:35 AM', author: 'Nurse Alex', details: 'Temp 37.2C, BP 135/85, HR 88' }
      ]
    },
    { 
      mrn: 'MRN-1102', firstName: 'Michael', lastName: 'Ross', dob: '1978-12-05', gender: 'M', phone: '(555) 555-0199', lastVisit: '2023-10-30', status: 'Results Pending', insurance: 'Medicare', balance: 150.00, acuity: 'High', allergies: ['Latex', 'Sulfa'],
      chiefComplaint: 'Chest Pain', vitalsSummary: '185/110 • 110bpm', activeProblems: ['Hypertension', 'T2DM'], currentMeds: ['Lisinopril 10mg', 'Metformin 500mg'],
      timeline: [
        { id: 'ev-4', type: 'Note', title: 'Triage Note', timestamp: 'Today, 08:00 AM', author: 'Nurse Alex', details: 'Substernal chest pressure. Diaphoretic.' },
        { id: 'ev-5', type: 'Order', title: 'ECG Ordered', timestamp: 'Today, 08:05 AM', author: 'Dr. Sarah', details: '12-Lead ECG - Stat', status: 'Completed' },
        { id: 'ev-6', type: 'Lab', title: 'Troponin I', timestamp: 'Today, 08:45 AM', author: 'Lab', details: 'Value: 0.02 ng/mL (Normal)', status: 'Final' }
      ]
    },
    { 
      mrn: 'MRN-4451', firstName: 'Emily', lastName: 'Blunt', dob: '1988-02-14', gender: 'F', phone: '(555) 222-3344', lastVisit: '2024-02-01', status: 'Discharged', insurance: 'UnitedHealth', balance: 0.00, acuity: 'Low', allergies: [] 
    },
    { 
      mrn: 'MRN-7721', firstName: 'Robert', lastName: 'Ford', dob: '1955-01-10', gender: 'M', phone: '(555) 333-1111', lastVisit: '2024-03-01', status: 'Pending Triage', insurance: 'Medicare', balance: 0.00, acuity: 'Critical', allergies: [] 
    },
  ];

  // Mock Doctors
  private _doctors: Doctor[] = [
    { id: 'dr-sarah', name: 'Dr. Sarah Mitchell', specialty: 'General Practice', status: 'Available', nextFreeSlot: '10:15 AM' },
    { id: 'dr-james', name: 'Dr. James Wilson', specialty: 'Cardiology', status: 'Busy', currentPatient: 'Michael Ross', nextFreeSlot: '11:00 AM' },
    { id: 'dr-chen', name: 'Dr. Emily Chen', specialty: 'Pediatrics', status: 'Away', nextFreeSlot: '1:30 PM' },
  ];

  // Mock Meds
  private _meds: MedicationTask[] = [
    { id: 'task-1', patientMrn: 'MRN-1102', patientName: 'Ross, Michael', drugName: 'Lisinopril', dose: '10mg PO', dueTime: '09:00 AM', status: 'Overdue' },
    { id: 'task-2', patientMrn: 'MRN-1102', patientName: 'Ross, Michael', drugName: 'Metoprolol', dose: '25mg PO', dueTime: '09:00 AM', status: 'Overdue' },
    { id: 'task-3', patientMrn: 'MRN-9932', patientName: 'Connor, Sarah', drugName: 'Zofran', dose: '4mg IV', dueTime: '10:00 AM', status: 'Due' },
  ];

  // Mock Vital Alerts
  private _alerts: VitalAlert[] = [
    { patientMrn: 'MRN-1102', patientName: 'Ross, Michael', room: 'ER-4', alert: 'BP: 185/110', severity: 'Critical' },
    { patientMrn: 'MRN-7721', patientName: 'Ford, Robert', room: 'Waiting', alert: 'HR: 120 bpm', severity: 'High' },
  ];

  // Mock Result Inbox
  private _resultsInbox: ResultInboxItem[] = [
    { id: 'res-1', patientName: 'Ross, Michael', patientMrn: 'MRN-1102', testName: 'Potassium', value: '6.2 mmol/L', flag: 'Critical', timestamp: '10m ago' },
    { id: 'res-2', patientName: 'Doe, John', patientMrn: 'MRN-8821', testName: 'WBC', value: '14.5', flag: 'High', timestamp: '25m ago' }
  ];

  // --- Mock Admin Data ---
  private _systemUsers: SystemUser[] = [
    { id: 'u1', name: 'Alex Admin', username: 'alex_admin', roles: ['Admin', 'Receptionist'], status: 'Active', lastLogin: 'Just now' },
    { id: 'u2', name: 'Dr. Sarah', username: 'sarah_md', roles: ['Doctor', 'Admin'], status: 'Active', lastLogin: '2h ago' },
    { id: 'u3', name: 'Nurse Jackie', username: 'jackie_rn', roles: ['Nurse'], status: 'Active', lastLogin: '1d ago' },
    { id: 'u4', name: 'Dr. James', username: 'james_md', roles: ['Doctor'], status: 'Locked', lastLogin: '3d ago' },
  ];

  private _auditLogs: AuditLog[] = [
    { id: 'log-101', timestamp: '2024-03-12 10:45:22', user: 'alex_admin', action: 'Update', entity: 'Invoice #1024', details: 'Adjustment applied', diff: { old: '100.00', new: '50.00' } },
    { id: 'log-100', timestamp: '2024-03-12 10:42:01', user: 'sarah_md', action: 'Login', entity: 'Auth System', details: 'Successful login from IP 192.168.1.50' },
    { id: 'log-099', timestamp: '2024-03-12 09:15:00', user: 'system', action: 'Auto-Backup', entity: 'Database', details: 'Hourly backup completed' },
    { id: 'log-098', timestamp: '2024-03-12 08:30:11', user: 'jackie_rn', action: 'Update', entity: 'Patient MRN-8821', details: 'Vitals recorded', diff: { old: 'null', new: '{temp: 37.5}' } },
  ];

  private _formSchemas: FormSchema[] = [
    { 
      id: 'frm-1', name: 'Triage - Standard', version: 3, lastUpdated: '2024-02-20', 
      schema: JSON.stringify([
        { key: 'temp', type: 'number', label: 'Temperature (C)', required: true },
        { key: 'hr', type: 'number', label: 'Heart Rate (bpm)', required: true },
        { key: 'complaint', type: 'text', label: 'Chief Complaint', required: true }
      ], null, 2)
    },
    { 
      id: 'frm-2', name: 'Covid-19 Screening', version: 1, lastUpdated: '2023-11-05', 
      schema: JSON.stringify([
        { key: 'cough', type: 'boolean', label: 'Dry Cough?' },
        { key: 'fever', type: 'boolean', label: 'Fever > 38C?' },
        { key: 'travel', type: 'text', label: 'Recent Travel' }
      ], null, 2)
    }
  ];

  patients = signal<Patient[]>(this._patients);
  doctors = signal<Doctor[]>(this._doctors);
  meds = signal<MedicationTask[]>(this._meds);
  alerts = signal<VitalAlert[]>(this._alerts);
  resultInbox = signal<ResultInboxItem[]>(this._resultsInbox);

  // Admin Signals
  users = signal<SystemUser[]>(this._systemUsers);
  auditLogs = signal<AuditLog[]>(this._auditLogs);
  formSchemas = signal<FormSchema[]>(this._formSchemas);
  
  // Waiting Room Queue (Receptionist View)
  getWaitingRoom() {
    return this.patients().filter(p => p.status === 'Checked In' || p.status === 'Waiting');
  }

  // Triage Queue (Nurse View - includes Waiting + Pending Triage)
  getTriageQueue() {
    return this.patients().filter(p => p.status === 'Pending Triage' || p.status === 'Checked In');
  }

  // Doctor's Active Queue
  getDoctorQueue() {
    // Patients who are ready for exam, in progress, or pending results
    return this.patients().filter(p => ['Ready for Exam', 'Results Pending', 'In Progress', 'Waiting'].includes(p.status));
  }

  getPatientByMrn(mrn: string) {
    return this.patients().find(p => p.mrn === mrn);
  }

  addPatient(patient: Patient) {
    this.patients.update(list => [...list, patient]);
  }

  addTimelineEvent(mrn: string, event: TimelineEvent) {
    this.patients.update(list => {
      const p = list.find(pat => pat.mrn === mrn);
      if (p) {
        if (!p.timeline) p.timeline = [];
        p.timeline = [event, ...p.timeline]; // Prepend
      }
      return [...list];
    });
  }

  // Admin Actions
  addUser(user: SystemUser) {
    this.users.update(list => [...list, user]);
  }

  updateFormSchema(id: string, newSchema: string) {
    this.formSchemas.update(list => list.map(f => f.id === id ? { ...f, schema: newSchema, version: f.version + 1 } : f));
  }
}
