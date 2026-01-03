# Product Requirements Document: HMS

## Executive Summary

The Open Source Hospital Management System (HMS) is a modern, modular platform aimed at democratizing enterprise-grade healthcare technology for small-to-medium institutions. By bridging the gap between expensive proprietary systems and inefficient paper-based workflows, the project provides a reliable, cost-free foundation that prioritizes clinical efficiency and operational scalability. The system is designed not just as a tool, but as a platform for a sustainable healthcare ecosystem.

### What Makes This Special

- **Internal-as-External Architecture**: Unlike monolithic legacy systems, this HMS treats internal modules (such as Lab and Pharmacy) as external integrations communicating via standard HL7/FHIR queues. This allows any module to be swapped or extended without breaking the core, ensuring the system scales gracefully from a small clinic to a hospital network.
- **Ecosystem Enablement**: The project is intentionally designed to be serviced and customized by local technology firms. This "local support economy" approach ensures that institutions have access to affordable, nearby expertise while contributing to a globally shared open-source core.
- **Clinical Reality Focus**: Moving away from administrative rigidity, the system prioritizes "Clinical Reality" through progressive data capture (allowing registration to happen as care is delivered) and a dynamic assessment engine that adapts to specific medical protocols without requiring code changes.

## Project Classification

**Technical Type:** Web Application
**Domain:** Healthcare
**Complexity:** High
**Project Context:** Greenfield - new project

This project is classified as high complexity due to the stringent requirements for patient data privacy (HIPAA/GDPR), clinical safety, and the need for robust interoperability. The technical approach leverages an asynchronous, message-driven architecture to handle the complex, non-linear workflows inherent in healthcare environments.

## Success Criteria

### User Success

*   **Zero Critical Blockers**: A user is never forced to abandon the system for a paper workflow during a patient interaction.
*   **Operational Speed**:
    *   **Patient Intake Time**: From walk-in to triage completion in under 5 minutes.
    *   **Fallback Efficiency**: Manual overrides (e.g., document uploads for missing features) take < 1 minute.
*   **Clinical Confidence**:
    *   **Alert Visibility**: Clinicians see 100% of critical "red flag" alerts (abnormal vitals) within their primary workspace without clicking away.
    *   **Information Availability**: Full patient context (history + current triage) is available in < 2 seconds.

### Business Success

*   **Ecosystem Activation**: At least 3 local technology partners offering support or managed hosting services within 6 months of MVP launch.
*   **Market Viability**: Successful deployment in 5 pilot clinics with zero "return to paper" incidents during the pilot phase.
*   **Cost Efficiency**: A projected 70% reduction in Total Cost of Ownership (TCO) for clinics compared to proprietary mid-market alternatives.

### Technical Success

*   **Architectural Validation**: Successful "hot-swap" or extension of the internal Lab module with an external HL7-compliant system without modifying the core codebase.
*   **Reliability**:
    *   **99.9% Uptime** during clinic operating hours.
    *   **Zero Data Loss**: Guaranteed delivery of all HL7/FHIR messages via the asynchronous Postgres queue.
*   **Deployment Velocity**: **Time-to-First-Patient** < 60 minutes (from server deploy to registering the first patient).

### Measurable Outcomes

*   **System Abandonment Rate**: < 1% of total visits reverted to paper due to system issues.
*   **Audit Completeness**: 100% of manual overrides have an associated reason code or file attachment.
*   **Performance**: < 200ms response time for loading the Clinical Dashboard.

## Product Scope

### MVP - Minimum Viable Product

The MVP focuses on the "Happy Path" for a small clinic visit:
1.  **IAM**: Role-based access (Doctor, Nurse, Staff, Admin) + Standalone IdP with JWT.
2.  **Registration**: Basic & Emergency registration (MRN generation).
3.  **Triage**: Vitals capture + Chief Complaint (Progressive Data Capture).
4.  **Clinical Core**: Patient Dashboard, Clinical Notes, and Basic CPOE (Labs/Meds).
5.  **Ancillary Integration**: Internal "stub" modules for Lab and Pharmacy communicating via HL7 queues (Result Entry = Manual).
6.  **Billing**: Basic "Clinical Invoice" (consultation + fixed price orders) with manual overrides.
7.  **Inventory**: Simple "Stock on Hand" tracking with auto-decrement on dispense.

### Growth Features (Post-MVP)

1.  **Advanced CPOE**: Drug-drug interaction checks and order sets.
2.  **Patient Portal**: Self-registration, appointment booking, and history view.
3.  **Insurance Module**: Basic claims generation (CMS-1500 support).
4.  **Telemedicine**: Integrated video consults via WebRTC.
5.  **Dynamic Form Builder UI**: Visual editor for admins to create assessment forms (currently JSON only).

### Vision (Future)

1.  **AI-Driven Diagnostics**: Predictive analytics for early warning scores (NEWS2).
2.  **IoT Integration**: Direct data ingestion from connected vitals monitors.
3.  **National Health Grid**: Direct integration with national health registries and HIEs (Health Information Exchanges).

## User Journeys

### Journey 1: The "Multi-Hat" Professional (Alex - Receptionist & Nurse)
**Context:** Alex works in a resource-constrained clinic where he covers both the front desk and initial nursing triage.
**The Story:**
A patient, Mr. Chen, walks in looking pale. Alex is logged in with his "Receptionist" role active. He hits "Emergency Registration," types "Chen" and "Male," and the system generates an MRN.
He then needs to take Mr. Chen's vitals. He doesn't log out. He clicks his profile avatar and toggles his active role to "Nurse" (a permission granted to him because he holds both roles). The dashboard transforms: the billing and scheduling widgets are replaced by the Triage and Clinical Assessment modules.
He selects the "Acute Chest Pain" protocol. As he enters a high BP reading, the field glows amber. He saves the triage note.
Later, when Mr. Chen is discharged, Alex toggles back to "Receptionist" to print the visit summary and handle the co-pay. The system allowed him to be two distinct professionals in one workflow without security friction.

### Journey 2: The "Clinical Cockpit" Decision (Dr. Sarah - Clinician)
**Context:** Dr. Sarah is in her office, reviewing her queue. She sees Mr. Chen's "High Priority" flag.
**The Story:**
She clicks Mr. Chen's name. The Patient Dashboard loads in under a second. She doesn't have to hunt for the vitals Alex just took; they are pinned to the top, with the high BP flagged in red. She reviews his history—he's hypertensive.
She opens the "Order Entry" panel. She types "Trop" and selects "Troponin I" from the auto-complete list. She adds a "Stat ECG" order. The system doesn't block her with billing pop-ups or insurance checks; it just confirms the order sent.
Later, a notification badge appears on her sidebar. The Lab has posted the result. She clicks it, confirms the diagnosis, and starts the treatment protocol. The system empowered her to act fast by keeping the tech out of her way.

### Journey 3: The "Audit Trail" Detective (Marcus - Billing Clerk)
**Context:** It's end-of-month reconciliation. Marcus is reviewing invoices before they are finalized.
**The Story:**
He flags an invoice for Patient #1024. The total seems low for the procedures listed. He opens the "Clinical Invoice" view. He sees a line item for a "Basic Metabolic Panel" that was manually price-adjusted from $50 to $0.
Hovering over the change, he sees the audit log: "User: Dr. Sarah. Reason: Indigent Patient Program / Fee Waived. Timestamp: Jan 12, 14:00."
Marcus smiles. No need to chase down Dr. Sarah or dig through emails. The justification is right there. He approves the invoice, knowing the financial trail is clean and compliant.

### Journey Requirements Summary

*   **Rapid Registration**: Ability to generate MRN with minimal data (Name/Gender) for emergencies.
*   **Multi-Role Assignment**: Capability to assign multiple distinct roles (e.g., Nurse AND Receptionist) to a single user account.
*   **Context-Aware Role Switching**: User interface mechanism to toggle the "Active Role," dynamically changing the visible modules and permissions without re-authentication.
*   **Dynamic Triage Forms**: Ability to select different assessment protocols (Standard vs. Acute) that trigger different data fields.
*   **Real-time Dashboard**: Instant updates across user sessions when patient status changes.
*   **Clinical Alerts**: Visual highlighting (amber/red) for abnormal values in vitals inputs and dashboard displays.
*   **Frictionless CPOE**: Order entry workflow that prioritizes speed and searchability over administrative validation.
*   **Audit Logging**: Immutable tracking of all financial changes, capturing User, Timestamp, and mandatory Reason Codes.

## Domain-Specific Requirements

### Healthcare Compliance & Regulatory Overview

HMS is designed as a modular, web-based Hospital Management System (HMS). For the initial version, the system is classified primarily as **Administrative & Clinical Workflow Support** rather than a diagnostic medical device. This classification streamlines the regulatory path by focusing on record-keeping, workflow orchestration, and data visibility for healthcare professionals.

### Key Domain Concerns

- **Regulatory Classification**: The system operates as a Class I / Exempt workflow tool. It does not provide automated diagnosis or treatment recommendations; it facilitates the capture and display of clinician-driven data.
- **Clinical Integrity**: Ensuring that the data Alex enters is exactly what Dr. Sarah sees is the primary safety concern. The system prioritizes data "fidelity" and "provenance" (knowing who entered what and when).
- **Patient Safety (Offline Path)**: In the event of system downtime, the primary fail-safe is the "Fallback Efficiency" policy, allowing for manual document uploads once the system returns, ensuring no data gap in the patient's permanent record.

### Compliance Requirements

- **HIPAA/GDPR Alignment**: Mandatory data encryption in transit and at rest.
- **Role-Based Data Segregation**: Access to specific patient records is strictly tied to active clinical or administrative roles.
- **Auditability**: Every clinical observation, triage note, and order must have an immutable audit trail (User ID + Timestamp + Action).

### Industry Standards & Best Practices

- **HL7/FHIR**: The core "Internal-as-External" architecture is built on FHIR-aligned JSON messaging to ensure future-proof interoperability with external Labs and Pharmacies.
- **ICD-10/LOINC**: The system will support standard terminology for diagnoses and lab observations to ensure data consistency.

### Required Expertise & Validation

- **Clinical Validation**: All dynamic assessment forms (e.g., CDC protocols) must be reviewed by a clinical subject matter expert to ensure they capture the necessary data points for clinical safety.
- **Data Integrity Testing**: Rigorous testing of the asynchronous message queue (Postgres-as-Queue) to ensure 100% message delivery reliability.

### Special Sections: Safety & Validation

- **Clinical Requirements**: Standardized data entry masks to prevent common input errors (e.g., preventing a pulse rate of 999).
- **Safety Measures**: Visual "Red Flag" alerts for abnormal vitals based on industry-standard ranges, intended as situational awareness tools for clinicians.

## Innovation & Novel Patterns

### Detected Innovation Areas

*   **Architectural Inversion ("Internal-as-External")**:
    *   **Concept**: Treating core internal modules (Lab, Pharmacy) as if they were third-party external systems from Day 1, communicating solely via asynchronous HL7/FHIR message queues.
    *   **Novelty**: Most HMS platforms are monolithic or tightly coupled. This approach enforces strict modularity, allowing a small clinic to eventually swap its internal "stub" lab module for a full-scale LIS integration without changing a single line of core code.
*   **Business Model Innovation (Local Support Economy)**:
    *   **Concept**: Explicitly designing the codebase and configuration tooling to be serviced by local, independent tech firms rather than a central vendor.
    *   **Novelty**: This challenges the "Vendor Lock-in" model of proprietary giants (Epic/Cerner), creating a distributed economic ecosystem around the software.
*   **Workflow Adaptability (Progressive Data Capture)**:
    *   **Concept**: A "No Dead Ends" policy where workflows like Registration or Triage can be paused, saved in partial states, or completed with minimal data (e.g., emergency registration) without blocking downstream clinical actions.

### Market Context & Competitive Landscape

*   **Proprietary Incumbents (Epic, Cerner)**: Highly integrated but rigid and expensive. Their architecture relies on massive, unified databases that make "swapping" components nearly impossible for smaller clients.
*   **Existing Open Source (OpenMRS, OpenEMR)**: Often struggle with the "Monolith vs. Microservices" tension. Your approach of using *standard interoperability protocols* (FHIR) as the *internal* communication bus is a forward-looking pattern that leapfrogs older open-source architectures.

### Validation Approach

*   **Latency Testing**: Rigorous load testing of the Postgres-based message queue to ensure that the "decoupled" architecture doesn't introduce perceptible lag for the end-user (Target: < 200ms roundtrip).
*   **"Swap" Drill**: A technical validation exercise where the internal "Stub Lab" is replaced by a mock external system to prove the interface holds without code changes.

### Risk Mitigation

*   **Performance Risk**: The asynchronous nature of queues could lead to "eventual consistency" issues where a doctor doesn't see a result immediately.
    *   *Mitigation*: Optimistic UI updates in the frontend and high-priority queue channels for critical alerts.
*   **Complexity Risk**: Local support firms might lack the expertise to manage a message-driven architecture.
    *   *Mitigation*: Providing a "One-Click Deploy" Docker container that encapsulates the complexity, exposing only simple configuration UIs.

## Web Application Specific Requirements

### Project-Type Overview

HMS is a modern web-based platform designed for high-performance clinical and administrative workflows. The technical architecture priorities are state management (for complex patient data), cross-role visibility, and reliability over public-facing features like SEO or social sharing.

### Technical Architecture Considerations

- **Frontend Framework**: Developed as a **Single Page Application (SPA)** using **Angular**. This ensures a "desktop-like" experience for clinicians (Dr. Sarah) and operational staff (Alex), allowing them to switch between modules without full page reloads.
- **State Management**: Robust RxJS-based state management is required to handle real-time patient data updates and role-aware UI transitions.
- **Browser Compatibility**: Optimized exclusively for **Google Chrome**. This allows the development team to leverage modern web standards and ensure a consistent experience across clinic workstations without the overhead of supporting legacy or non-standard browsers.

### Platform & Performance Requirements

- **Real-Time Data**: Implementation of **periodic polling** (e.g., every 30-60 seconds) for the main patient dashboard and clinical queues. This ensures clinicians see status updates (e.g., Alex finishing a triage) without excessive WebSocket complexity for the MVP.
- **SEO & Search Visibility**: SEO is **not required**. The application will be configured to block search engine crawlers (`robots.txt`) to protect the privacy and security of the clinical portal.
- **Accessibility & UI/UX**: While strict WCAG compliance is deferred, the UI will prioritize high-contrast color schemes and clear typography to ensure readability in busy, high-stress clinic environments.

### Implementation Specific Requirements

- **Responsive Design**: While primary usage is on desktop workstations, the interface must be functional on tablet-sized screens to support "on-the-go" triage or ward management.
- **Performance Target**: Initial dashboard load in < 2 seconds on a standard clinic broadband connection.
- **Security Protocols**: All traffic served over HTTPS with strict Content Security Policy (CSP) headers to prevent XSS and other web-based vulnerabilities.

## Project Scoping & Phased Development

### MVP Strategy & Philosophy

**MVP Approach:** **Platform MVP**
The priority for Phase 1 is to validate the "Internal-as-External" message-driven architecture. By building a robust FHIR-aligned communication bus first, we ensure that the system can scale and be customized by local tech partners. Clinical features (Registration, Triage, CPOE) will be implemented as independent modules to prove this modularity.

**Resource Requirements:**
- **Core Team**: 1 Backend (ASP.NET Core / .NET 9, Postgres), 1 Frontend (Angular/RxJS), 1 Clinical Subject Matter Expert (Part-time).
- **Infrastructure**: Docker-based deployment for easy local partner setup.

### MVP Feature Set (Phase 1)

**Core User Journeys Supported:**
- **Journey 1 (Alex)**: Multi-role switching, emergency registration, and vitals capture.
- **Journey 2 (Dr. Sarah)**: Clinical dashboard summary and simple order entry (Labs/Meds).
- **Journey 3 (Marcus)**: Financial audit logs for clinical invoices.

**Must-Have Capabilities:**
- **IAM**: Role-based access with context-aware role switching.
- **Registration & Triage**: MRN generation and dynamic assessment forms.
- **Clinical Core**: Unified Patient Dashboard and basic CPOE.
- **Postgres-as-Queue**: Asynchronous message bus for HL7/FHIR communication.
- **Billing/Inventory Stubs**: Basic invoice generation and manual stock tracking.

### Post-MVP Features

**Phase 2 (Growth):**
- **External Integrations**: Connecting a real external LIS via the HL7 queue.
- **Patient Portal**: Self-registration and history access for patients.
- **Enhanced CPOE**: Decision support (drug-drug interaction checks).

**Phase 3 (Expansion):**
- **Insurance Adjudication**: Full RCM (Revenue Cycle Management) workflow.
- **Telemedicine**: Integrated video consults.
- **IoT Vitals**: Direct data ingestion from connected medical devices.

### Risk Mitigation Strategy

**Technical Risks:**
- **Queue Latency**: Mitigation through RxJS-based optimistic UI updates and performance monitoring of the Postgres queue.
- **Modular Complexity**: Mitigation through strictly defined FHIR-based JSON schemas for all inter-module communication.

**Market Risks:**
- **Adoption**: Mitigation by ensuring the "Record-Keeping" UI is significantly faster than paper, proven in the 5 pilot clinics.
- **Ecosystem Buy-in**: Mitigation by providing comprehensive "Module Templates" for local tech partners early in the cycle.

**Resource Risks:**
- **Scope Creep**: Mitigation by strictly deferring any non-core clinical features (like advanced insurance or AI) to Phase 3.

## Functional Requirements

### 1. Identity & Access Management (IAM)
- **FR1**: System can assign multiple distinct roles (Doctor, Nurse, Staff, Admin, Billing Clerk) to a single user account.
- **FR2**: Authenticated User can switch their "Active Role" dynamically without logging out.
- **FR3**: System can restrict module and data access based on the "Active Role" and its associated permissions.
- **FR4**: System can authenticate users via a standalone Identity Provider (IdP) issuing JWTs.
- **FR5**: Admin can manage users, roles, and granular permission sets.

### 2. Patient Registration & Identity
- **FR6**: Operational Staff can perform "Emergency Registration" using minimal data (Name/Gender) to generate a unique Medical Record Number (MRN) instantly.
- **FR7**: Operational Staff can complete a "Full Registration" by adding demographic, insurance, and contact details to an existing MRN.
- **FR8**: System can uniquely identify patients across their lifetime using a permanent MRN.
- **FR9**: System can prevent duplicate MRN generation during registration via data matching.

### 3. Triage & Clinical Assessment
- **FR10**: Nurse can capture and save patient vitals (BP, Heart Rate, Temp, etc.).
- **FR11**: Nurse can document a "Chief Complaint" or reason for visit.
- **FR12**: Nurse can select and complete specific "Assessment Protocols" (e.g., Acute Chest Pain, Standard Check-up) from a dynamic list.
- **FR13**: System can flag abnormal vitals with visual alerts based on predefined clinical thresholds.
- **FR14**: System can store clinical assessment definitions (questions/fields) as dynamic, non-hardcoded templates.

### 4. Clinical Workspace & CPOE
- **FR15**: Clinician can view a consolidated "Patient Dashboard" showing history, current vitals, and active orders.
- **FR16**: Clinician can enter and manage secure "Clinical Notes" during a consultation.
- **FR17**: Clinician can place orders for Lab Tests or Medications via a search-driven interface.
- **FR18**: Clinician can prioritize orders (e.g., "Stat" vs. "Routine").
- **FR19**: Clinician can view real-time status updates for patient priority and order fulfillment.

### 5. Asynchronous Interoperability (Queue Management)
- **FR20**: System can transmit clinical orders to ancillary modules (Lab/Pharmacy) via an asynchronous message queue.
- **FR21**: System can receive and associate results from ancillary modules back to the patient record via an asynchronous queue.
- **FR22**: System can ensure 100% delivery of inter-module messages using a persistent "Internal-as-External" communication bus.
- **FR23**: System can handle "Partial State" saves for clinical workflows to support progressive data capture.

### 6. Billing & Financial Audit
- **FR24**: System can automatically generate a "Clinical Invoice" based on consultation fees and fulfilled orders.
- **FR25**: Billing Clerk can view a task queue of pending and finalized invoices.
- **FR26**: Authorized User (Doctor/Clerk) can manually adjust invoice line items.
- **FR27**: System can enforce mandatory "Reason Codes" and notes for any manual financial adjustment.
- **FR28**: System can maintain an immutable audit log of all financial changes, including user, timestamp, and justification.

### 7. Inventory Management
- **FR29**: System can track current "Stock on Hand" and pricing for medications and medical supplies.
- **FR30**: System can automatically decrement inventory stock levels upon a "Dispense" action in the Pharmacy module.
- **FR31**: Admin can manually update stock levels and item pricing.

### 8. System & Configuration
- **FR32**: Admin can configure system settings and dynamic form templates without modifying the core codebase.
- **FR33**: System can block search engine crawlers from clinical and administrative pages via robots.txt configuration.
