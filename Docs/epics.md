# HMS - Epic Breakdown

## Overview

This document provides the complete epic and story breakdown for HMS, decomposing the requirements from the PRD, UX Design if it exists, and Architecture requirements into implementable stories.

## Requirements Inventory

### Functional Requirements

FR1: System can assign multiple distinct roles (Doctor, Nurse, Staff, Admin, Billing Clerk) to a single user account.
FR2: Authenticated User can switch their "Active Role" dynamically without logging out.
FR3: System can restrict module and data access based on the "Active Role" and its associated permissions.
FR4: System can authenticate users via a standalone Identity Provider (IdP) issuing JWTs.
FR5: Admin can manage users, roles, and granular permission sets.
FR6: Operational Staff can perform "Emergency Registration" using minimal data (Name/Gender) to generate a unique Medical Record Number (MRN) instantly.
FR7: Operational Staff can complete a "Full Registration" by adding demographic, insurance, and contact details to an existing MRN.
FR8: System can uniquely identify patients across their lifetime using a permanent MRN.
FR9: System can prevent duplicate MRN generation during registration via data matching.
FR10: Nurse can capture and save patient vitals (BP, Heart Rate, Temp, etc.).
FR11: Nurse can document a "Chief Complaint" or reason for visit.
FR12: Nurse can select and complete specific "Assessment Protocols" (e.g., Acute Chest Pain, Standard Check-up) from a dynamic list.
FR13: System can flag abnormal vitals with visual alerts based on predefined clinical thresholds.
FR14: System can store clinical assessment definitions (questions/fields) as dynamic, non-hardcoded templates.
FR15: Clinician can view a consolidated "Patient Dashboard" showing history, current vitals, and active orders.
FR16: Clinician can enter and manage secure "Clinical Notes" during a consultation.
FR17: Clinician can place orders for Lab Tests or Medications via a search-driven interface.
FR18: Clinician can prioritize orders (e.g., "Stat" vs. "Routine").
FR19: Clinician can view real-time status updates for patient priority and order fulfillment.
FR20: System can transmit clinical orders to ancillary modules (Lab/Pharmacy) via an asynchronous message queue.
FR21: System can receive and associate results from ancillary modules back to the patient record via an asynchronous queue.
FR22: System can ensure 100% delivery of inter-module messages using a persistent "Internal-as-External" communication bus.
FR23: System can handle "Partial State" saves for clinical workflows to support progressive data capture.
FR24: System can automatically generate a "Clinical Invoice" based on consultation fees and fulfilled orders.
FR25: Billing Clerk can view a task queue of pending and finalized invoices.
FR26: Authorized User (Doctor/Clerk) can manually adjust invoice line items.
FR27: System can enforce mandatory "Reason Codes" and notes for any manual financial adjustment.
FR28: System can maintain an immutable audit log of all financial changes, including user, timestamp, and justification.
FR29: System can track current "Stock on Hand" and pricing for medications and medical supplies.
FR30: System can automatically decrement inventory stock levels upon a "Dispense" action in the Pharmacy module.
FR31: Admin can manually update stock levels and item pricing.
FR32: Admin can configure system settings and dynamic form templates without modifying the core codebase.
FR33: System can block search engine crawlers from clinical and administrative pages via robots.txt configuration.

### NonFunctional Requirements

NFR1: Performance: < 200ms response time for critical actions; < 2s dashboard load time.
NFR2: Reliability: 99.9% uptime during clinic operating hours; Zero data loss for HL7/FHIR messages.
NFR3: Security: HIPAA/GDPR alignment; Mandatory data encryption in transit and at rest; Immutable audit trails.
NFR4: Usability: "No Dead Ends" policy; Fallback efficiency (manual overrides in < 1 minute); Optimistic UI feedback.
NFR5: Interoperability: HL7/FHIR-native internal communication; FHIR-aligned JSON messaging.
NFR6: Scalability: "Internal-as-External" modularity to support scaling from clinic to hospital networks.

### Additional Requirements

- Architecture: .NET solution + Angular (Backend: ASP.NET Core / .NET 9; Frontend: Angular 21)
- Architecture: "Internal-as-External" async messaging pattern using Postgres Queue (SKIP LOCKED)
- Architecture: Standalone Identity Provider (IdP) module issuing context-aware JWTs
- Architecture: Optimistic UI for asynchronous operations
- UX: "Clinical Cockpit" with 3-column "Master-Detail" layout (Navigation, Pinned Context & Timeline, Dynamic Action Block)
- UX: High-density data grid with spreadsheet-like navigation and "stat-glow" alerts
- UX: Context-aware role switcher on Dashboard
- UX: Patient Pinned Ribbon for at-a-glance critical data
- Project Context: Strict `camelCase` for all API JSON payloads; `snake_case` for database objects (Postgres); C# code uses `PascalCase`.
- Project Context: All Angular components MUST be `standalone: true` and use `OnPush` change detection.

### FR Coverage Map

- **FR1 (Multi-role):** Epic 4
- **FR2 (Role switching):** Epic 4
- **FR3 (Role restrictions):** Epic 4
- **FR4 (IdP Auth):** Epic 1
- **FR5 (Admin RBAC):** Epic 1
- **FR6 (Emergency Reg):** Epic 2
- **FR7 (Full Reg):** Epic 2
- **FR8 (MRN):** Epic 2
- **FR9 (Duplicates):** Epic 2
- **FR10 (Vitals):** Epic 3
- **FR11 (Chief Complaint):** Epic 3
- **FR12 (Protocols):** Epic 3
- **FR13 (Abnormal Alerts):** Epic 3
- **FR14 (Dynamic Templates):** Epic 3
- **FR15 (Dashboard):** Epic 4
- **FR16 (Notes):** Epic 4
- **FR17 (Order Entry):** Epic 4
- **FR18 (Priority):** Epic 4
- **FR19 (Status Updates):** Epic 4
- **FR20 (Order Transmit):** Epic 5
- **FR21 (Result Receive):** Epic 5
- **FR22 (Guaranteed Delivery):** Epic 5
- **FR23 (Partial States):** Epic 3
- **FR24 (Invoice Gen):** Epic 7
- **FR25 (Invoice Queue):** Epic 7
- **FR26 (Adjustments):** Epic 7
- **FR27 (Reason Codes):** Epic 7
- **FR28 (Audit Log):** Epic 7
- **FR29 (Stock Tracking):** Epic 6
- **FR30 (Auto-decrement):** Epic 6
- **FR31 (Pricing):** Epic 6
- **FR32 (System Config):** Epic 8
- **FR33 (Robots.txt):** Epic 1

## Epic List

### Epic 1: Foundations & Secure Access
Establish the angular workspace, HMS.API IdP module, and JWT-based authentication to ensure users can securely log in and admins can manage roles and permissions.
**FRs covered:** FR4, FR5, FR33

### Epic 2: Patient Identity & Emergency Registration
Implement the MRN generation and the progressive registration workflow so that staff can uniquely identify patients and perform rapid intake for emergency cases.
**FRs covered:** FR6, FR7, FR8, FR9

### Epic 3: Clinical Triage & Dynamic Assessment
Implement the Triage module with vital signs capture and dynamic form engine so that nurses can document the initial clinical state and receive alerts for abnormal vitals.
**FRs covered:** FR10, FR11, FR12, FR13, FR14, FR23

### Epic 4: Clinical Consultation & The "Cockpit"
Build the unified dashboard, clinical notes, and order entry interface with role-switching logic so that doctors can review history, document visits, and place orders within a high-density, context-aware interface.
**FRs covered:** FR1, FR2, FR3, FR15, FR16, FR17, FR18, FR19

### Epic 5: Reliable Asynchronous Interoperability
Implement the PostgreSQL-based message bus using the "Internal-as-External" pattern so that orders and results are reliably transmitted between modules without blocking clinical workflows.
**FRs covered:** FR20, FR21, FR22

### Epic 6: Pharmacy Fulfillment & Stock Management
Implement medication dispensing and real-time inventory tracking so that pharmacists can fulfill orders and the system automatically maintains accurate stock levels and pricing.
**FRs covered:** FR29, FR30, FR31

### Epic 7: Clinical Invoicing & Financial Audit
Build the billing module with automatic invoice generation and immutable audit logs so that billing clerks can manage invoices and perform adjustments with a transparent, compliant audit trail.
**FRs covered:** FR24, FR25, FR26, FR27, FR28

### Epic 8: System Configuration & Administration
Implement global settings and template management so that administrators can customize clinical protocols and system behavior without code changes.
**FRs covered:** FR32

## Epic 1: Foundations & Secure Access

Establish the solution scaffolding, Standalone IdP, and JWT-based authentication to ensure users can securely log in and admins can manage roles and permissions.

### Story 1.1: Project Initialization & Solution Setup

As a Developer,
I want to initialize the .NET solution and Angular application,
So that the team has a pre-configured development environment.

**Acceptance Criteria:**

**Given** The developer has Node.js, the .NET 9 SDK, and Docker installed
**When** They run the initialization commands defined in the Architecture
**Then** A new `.sln` and backend projects are created under `Backend/HMS_API` and the Angular app is created in `Frontend/HMS_UI`
**And** The `docker-compose.yml` successfully spins up PostgreSQL 18.1
**And** The frontend and backend can be started via `npm start` (frontend) and `dotnet run` (backend)

### Story 1.2: Shared Message Bus Infrastructure

As a Backend Developer,
I want to implement the shared message bus library using PostgreSQL,
So that modules can communicate asynchronously without direct dependencies.

**Acceptance Criteria:**

**Given** The PostgreSQL database is running
**When** The `message-bus` library is initialized
**Then** It should create the `app_events` table if not exists
**And** It should provide a `publish(event: str, payload: dict)` method
**And** It should provide a `consume(event_type: str)` method using `SKIP LOCKED` logic
**And** Unit tests verify that messages are persisted and retrieved in FIFO order

### Story 1.3: Standalone Identity Provider (IdP) Module

As a Security Architect,
I want to build the Auth module with JWT issuance,
So that users can authenticate and receive secure tokens.

**Acceptance Criteria:**

**Given** A user provides valid credentials
**When** They call `POST /auth/login`
**Then** The system returns a JWT containing `sub` (user_id) and `roles` list
**And** The system supports a `refresh_token` flow
**And** Invalid credentials return HTTP 401
**And** C# DTOs and model validation (data annotations or FluentValidation) validate the login request payload

### Story 1.4: Frontend Authentication & Role Interceptors

As a Frontend Developer,
I want to implement the Angular Auth Service and Interceptors,
So that every API request includes the JWT and handles 401 errors gracefully.

**Acceptance Criteria:**

**Given** A logged-in user with a stored JWT
**When** The application makes an HTTP request to the API
**Then** An `HttpInterceptor` automatically attaches the `Authorization: Bearer <token>` header
**And** If the API returns 401, the interceptor redirects to the login page
**And** An `AuthGuard` prevents access to protected routes if no token is present

## Epic 2: Patient Identity & Emergency Registration

Implement the MRN generation and the progressive registration workflow so that staff can uniquely identify patients and perform rapid intake for emergency cases.

### Story 2.1: Patient Module & Database Schema

As a Backend Developer,
I want to create the Patient module and database tables,
So that patient demographic data can be persisted securely.

**Acceptance Criteria:**

**Given** The `Backend/HMS_API` is running
**When** The application starts
**Then** EF Core migrations (or startup scripts) create the `pat_patients` table
**And** The table includes columns for `mrn` (unique, indexed), `first_name`, `last_name`, `dob`, and `gender`
**And** C# DTOs/models exist for `PatientCreate` and `PatientResponse`

### Story 2.2: Emergency Registration API & Logic

As a Backend Developer,
I want to implement the Emergency Registration endpoint,
So that staff can create a patient record with minimal data.

**Acceptance Criteria:**

**Given** An authorized user (Role: Receptionist or Nurse)
**When** They POST to `/patients/emergency` with only Name and Gender
**Then** The system generates a unique MRN (e.g., "MRN-2024-0001")
**And** A new patient record is created with status "Emergency"
**And** A `Patient.Created` event is published to the Message Bus
**And** The API returns the new patient details including the MRN

### Story 2.3: Patient Registration Frontend

As a Receptionist,
I want a rapid registration form in the web client,
So that I can quickly register a patient without navigating complex menus.

**Acceptance Criteria:**

**Given** I am on the Dashboard
**When** I click "Emergency Registration"
**Then** A modal opens requesting only First Name, Last Name, and Gender
**When** I submit the form
**Then** I see a loading spinner (Optimistic UI handled)
**And** On success, the modal closes and a success toast appears with the new MRN
**And** The patient is added to my active queue

### Story 2.4: Full Registration & Demographics

As a Receptionist,
I want to update an existing patient record with full demographics,
So that we have complete contact and insurance information.

**Acceptance Criteria:**

**Given** An existing patient record
**When** I send a PATCH request to `/patients/{mrn}`
**Then** I can update address, phone, email, and insurance details
**And** The system validates the input formats (e.g., email regex)
**And** An audit log entry is created for the update

## Epic 3: Clinical Triage & Dynamic Assessment

Implement the Triage module with vital signs capture and dynamic form engine so that nurses can document the initial clinical state and receive alerts for abnormal vitals.

### Story 3.1: Clinical Module & Dynamic Form Schema

As a Backend Developer,
I want to create the relational schema for dynamic forms,
So that we can build forms from reusable field definitions.

**Acceptance Criteria:**

**Given** The PostgreSQL database
**When** The Clinical module initializes
**Then** Tables are created for:
*   `clin_field_definitions` (Reusable library)
*   `clin_form_templates` (Form headers)
*   `clin_form_fields` (Mapping fields to templates)
*   `clin_assessments` (Instance of a filled form)
*   `clin_assessment_values` (Actual data)
**And** FK constraints ensure data integrity

### Story 3.2: Dynamic Form Builder API

As an Admin,
I want to define clinical forms via API,
So that I can create new triage protocols without code changes.

**Acceptance Criteria:**

**Given** I am an Administrator
**When** I POST a JSON schema to `/clinical/templates`
**Then** The system validates the schema structure
**And** Saves the template with a unique ID
**And** Clinicians can retrieve this template by ID to render a form

### Story 3.3: Triage Workflow & Vitals Capture

As a Nurse,
I want to record patient vitals during triage,
So that the doctor has baseline data.

**Acceptance Criteria:**

**Given** I am viewing a patient in the Triage view
**When** I enter Blood Pressure, Heart Rate, and Temperature
**Then** The system saves these values to the `clin_assessments` table
**And** The fields validate numeric ranges (e.g., HR must be 0-300)
**And** Partial data is saved as a "Draft" if I don't click "Finalize"

### Story 3.4: Vital Signs Alerting Logic

As a Nurse,
I want immediate visual feedback on abnormal vitals,
So that I can identify critical patients instantly.

**Acceptance Criteria:**

**Given** I am entering vitals
**When** I enter a value outside the normal range (e.g., Temp > 38C)
**Then** The input field border turns Red or Amber immediately
**And** A warning icon appears next to the field
**And** The "Chief Complaint" summary is flagged as "High Priority" if critical thresholds are met

## Epic 4: Clinical Consultation & The "Cockpit"

Build the unified dashboard, clinical notes, and order entry interface with role-switching logic so that doctors can review history, document visits, and place orders within a high-density, context-aware interface.

### Story 4.1: Clinical Cockpit Shell & Role Switcher

As a User with multiple roles,
I want to switch between "Nurse" and "Receptionist" views,
So that I see only the tools relevant to my current task.

**Acceptance Criteria:**

**Given** I have multiple roles assigned
**When** I select a new role from the Dashboard dropdown
**Then** The UI layout reconfigures (e.g., Billing widgets hide, Clinical widgets appear)
**And** The API token context is updated (or re-fetched) to reflect the active role permissions
**And** No full page reload is required

### Story 4.2: Patient Dashboard & Timeline

As a Doctor,
I want a consolidated view of the patient's history and current status,
So that I can make informed decisions quickly.

**Acceptance Criteria:**

**Given** I have selected a patient
**When** The Dashboard loads
**Then** I see a "Pinned Ribbon" with demographics and allergies
**And** I see a chronological timeline of past visits and triage notes
**And** The data loads in under 2 seconds

### Story 4.3: Clinical Notes Editor

As a Doctor,
I want to write free-text clinical notes,
So that I can document the consultation.

**Acceptance Criteria:**

**Given** I am in the Clinical Cockpit
**When** I type in the "Clinical Notes" block
**Then** The text is auto-saved locally every few seconds
**And** I can mark the note as "Final" to persist it to the database
**And** Finalized notes become read-only and immutable

### Story 4.4: Order Entry (CPOE) Interface

As a Doctor,
I want to search for and order Lab tests,
So that the patient can get diagnosed.

**Acceptance Criteria:**

**Given** I am in the Order Entry block
**When** I type "CBC" into the search bar
**Then** A list of matching Lab tests appears
**When** I select a test and click "Order"
**Then** An "Order.Created" event is sent to the API
**And** The UI shows an "Ordered" badge immediately (Optimistic UI)

## Epic 5: Reliable Asynchronous Interoperability

Implement the PostgreSQL-based message bus using the "Internal-as-External" pattern so that orders and results are reliably transmitted between modules without blocking clinical workflows.

### Story 5.1: Message Bus Publisher & Consumer Logic

As a Developer,
I want to implement the core Event Publisher and Consumer services,
So that modules can exchange data reliably.

**Acceptance Criteria:**

**Given** The message bus library from Story 1.2
**When** A service calls `publish_event()`
**Then** A row is inserted into `app_events` with status `pending`
**When** A worker calls `consume_events()`
**Then** It retrieves `pending` events for its topic
**And** Updates status to `processing` (atomic lock)
**And** Updates to `completed` upon success

### Story 5.2: Order Processing Worker

As a System,
I want to process "Order.Created" events,
So that the Lab module is notified of new work.

**Acceptance Criteria:**

**Given** A new "Order.Created" event in the queue
**When** The Lab Worker processes the event
**Then** It creates a corresponding record in the `lab_orders` table
**And** It acknowledges the event as processed
**And** If the Lab module is down (simulated), the event remains in the queue for retry

### Story 5.3: Result Processing Worker

As a System,
I want to process "Result.Available" events from the Lab,
So that the clinical dashboard is updated.

**Acceptance Criteria:**

**Given** A "Result.Available" event from the Lab module
**When** The Clinical Worker processes the event
**Then** It updates the patient's record with the result data
**And** Triggers a notification to the ordering doctor
**And** The result appears on the Patient Timeline

## Epic 6: Pharmacy Fulfillment & Stock Management

Implement medication dispensing and real-time inventory tracking so that pharmacists can fulfill orders and the system automatically maintains accurate stock levels and pricing.

### Story 6.1: Inventory Module & Stock Schema

As a Backend Developer,
I want to create the Inventory module schema,
So that we can track medication stock levels.

**Acceptance Criteria:**

**Given** The Postgres database
**When** The Inventory module initializes
**Then** An `inv_items` table is created with `sku`, `name`, `quantity`, and `unit_price`
**And** An `inv_transactions` table is created to log stock changes

### Story 6.2: Pharmacy Dispensing Workflow

As a Pharmacist,
I want to dispense a medication for an order,
So that the patient receives their treatment.

**Acceptance Criteria:**

**Given** A pending Medication Order
**When** I click "Dispense"
**Then** The system checks if sufficient stock exists
**And** Decrements the stock count in `inv_items`
**And** Publishes a "Medication.Dispensed" event to the bus
**And** Prevents dispensing if stock is zero

### Story 6.3: Stock Management Interface

As an Inventory Manager,
I want to manually update stock levels,
So that physical inventory matches the system.

**Acceptance Criteria:**

**Given** I am authenticated as an Inventory Manager
**When** I view the Stock Grid
**Then** I can edit the "Quantity" cell directly (Inline Edit)
**When** I save the change
**Then** The system records the adjustment in `inv_transactions` with my user ID

## Epic 7: Clinical Invoicing & Financial Audit

Build the billing module with automatic invoice generation and immutable audit logs so that billing clerks can manage invoices and perform adjustments with a transparent, compliant audit trail.

### Story 7.1: Billing Module & Invoice Schema

As a Backend Developer,
I want to create the Billing module schema,
So that financial transactions can be stored.

**Acceptance Criteria:**

**Given** The Postgres database
**When** The Billing module initializes
**Then** Tables for `bil_invoices` and `bil_line_items` are created
**And** An `audit_logs` table is created to track all financial changes

### Story 7.2: Automated Invoice Generation

As a System,
I want to auto-create invoices from clinical events,
So that billing is accurate and immediate.

**Acceptance Criteria:**

**Given** A "Medication.Dispensed" or "Lab.Completed" event
**When** The Billing Worker processes the event
**Then** It looks up the price from the Inventory/Lab module
**And** Adds a line item to the patient's active invoice
**And** Calculates the new total

### Story 7.3: Invoice Adjustment & Audit Log

As a Billing Clerk,
I want to manually adjust an invoice price,
So that I can apply discounts or correct errors.

**Acceptance Criteria:**

**Given** An active invoice
**When** I change a line item price
**Then** The system forces me to select a "Reason Code"
**And** The change is saved
**And** An immutable record is written to `audit_logs` capturing Who, When, Old Price, New Price, and Reason

## Epic 8: System Configuration & Administration

Implement global settings and template management so that administrators can customize clinical protocols and system behavior without code changes.

### Story 8.1: System Configuration Interface

As an Admin,
I want a UI to manage global system settings,
So that I can configure the clinic's name and defaults.

**Acceptance Criteria:**

**Given** I am an Administrator
**When** I navigate to System Settings
**Then** I can update the Clinic Name, Default Currency, and Timezone
**And** These settings are cached and applied globally

## Epic 9: Usability & Enhancements

Implement high-value, low-complexity improvements to enhance clinician efficiency, operational observability, and developer experience.

### Story 9.1: Clinical Macros (Dot Phrases)

As a Doctor,
I want to use short commands (dot phrases) to insert common blocks of text,
So that I can document clinical notes faster and more consistently.

**Acceptance Criteria:**

**Given** I am typing in the Clinical Note Editor
**When** I type `.` followed by a keyword (e.g., `.lungClear`)
**Then** The keyword is instantly replaced with a preset text block
**And** An autocomplete menu helps me find available macros

### Story 9.2: Patient Search & Command Palette

As a Power User,
I want a global search bar accessible by keyboard shortcut (`Ctrl+K`),
So that I can find a patient or navigate to a module without using the mouse.

**Acceptance Criteria:**

**Given** I am anywhere in the application
**When** I press `Ctrl+K`
**Then** A modal search bar appears
**When** I search for a patient name or menu item
**Then** I can navigate directly to the result

### Story 9.3: Audit Log Viewer

As an Compliance Officer,
I want to view a history of critical system actions,
So that I can investigate unauthorized changes or errors.

**Acceptance Criteria:**

**Given** I am an Administrator
**When** I view the Audit Logs page
**Then** I can filter logs by User, Date, or Entity Type
**And** I see a paginated list of all recorded changes

### Story 9.4: Inventory Low Stock Alerts

As an Inventory Manager,
I want to be notified when items run low,
So that I can reorder before we run out.

**Acceptance Criteria:**

**Given** An inventory item with a `min_reorder_level`
**When** Dispensing reduces the stock below this level
**Then** A system alert is triggered for the Inventory Manager

### Story 9.5: System Health & Diagnostics

As a DevOps Engineer,
I want an automated health check endpoint,
So that I know if the database or queue infrastructure is down.

**Acceptance Criteria:**

**Given** The application is running
**When** I call `/health`
**Then** I receive a 200 OK status with DB and Queue connectivity metrics

### Story 9.6: Database Seeder

As a Developer,
I want a script to populate the database with realistic test data,
So that I can demo the application without manual data entry.

**Acceptance Criteria:**

**Given** A fresh database
**When** I run the seeder command
**Then** The database is populated with dummy patients, providers, and inventory items

### Story 9.7: Patient Visit Summary PDF

As a Patient,
I want a PDF summary of my visit,
So that I have a record of my vitals and prescriptions.

**Acceptance Criteria:**

**Given** A completed patient visit
**When** I click "Print Summary"
**Then** A PDF is generated containing Vitals, Notes, and Orders

## Epic 10: Scheduling & Care Coordination

Manage the patient journey through Appointments, Episodes of Care, and Consultation tracking.

### Story 10.1: Appointment Scheduling

As a Receptionist,
I want to schedule an appointment for a patient,
So that they can see a doctor at a specific time.

**Acceptance Criteria:**

**Given** I select a Doctor and Date
**When** I book a slot
**Then** The system prevents double-booking
**And** A record is saved to `sch_appointments`

### Story 10.2: Episode of Care Management

As a Doctor,
I want to group related visits into an "Episode of Care" (e.g., "Pregnancy 2024"),
So that I can track a condition over time.

**Acceptance Criteria:**

**Given** A patient with a long-term condition
**When** I create an Episode
**Then** I can link multiple Consultations to it

### Story 10.3: Consultation Lifecycle

As a Doctor,
I want to explicitly "Start" and "End" a consultation,
So that we track the actual encounter duration.

**Acceptance Criteria:**

**Given** A scheduled appointment
**When** I click "Start Visit"
**Then** A `clin_consultations` record is created
**When** I click "Finish"
**Then** The consultation is marked as closed


