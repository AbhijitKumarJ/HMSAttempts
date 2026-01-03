# Product Brief: HMS

**Date:** 2026-01-03
**Author:** Abhijit
**Status:** Draft

## Executive Summary

The **Open Source Web-Based Hospital Management System (HMS)** aims to democratize access to enterprise-grade healthcare technology. By providing a reliable, cost-free, and highly customizable software foundation, this project addresses the critical gap where small-to-medium healthcare institutions are priced out of proprietary markets or forced into rigid, ill-fitting workflows. This solution is not just a product but a platform enablement strategy: it empowers local technology firms to offer affordable customization and support services, thereby creating a sustainable ecosystem that ultimately improves patient care and operational efficiency in resource-constrained environments.

---

## Core Vision

### Problem Statement

Small hospitals, nursing homes, and clinics globally face a binary choice: purchase expensive, rigid proprietary software that drains resources and dictates workflows, or rely on fragmented, inefficient paper-based systems that compromise patient safety. Current open-source alternatives often suffer from complexity, lack of maintainability, or "abandonware" status, failing to provide a viable professional alternative.

### Problem Impact

*   **For Institutions:** Inability to digitize operations leads to administrative overhead, revenue leakage, and inability to meet modern compliance standards (HIPAA/GDPR).
*   **For Patients:** Fragmented records result in slower care, potential medical errors, and a disjointed healthcare experience.
*   **For the Industry:** A lack of a standard, modern open-source core prevents local tech firms from serving the healthcare market effectively, as they cannot afford to build a compliant HMS from scratch.

### Why Existing Solutions Fall Short

*   **Proprietary Giants (Epic, Cerner):** Prohibitively expensive and inflexible for smaller providers.
*   **Existing Open Source:** Often built on outdated tech stacks, lack distinct modularity, or require massive effort to customize, making them "free like a puppy, not free like beer."

### Proposed Solution

A **modern, modular, web-based HMS** built on a robust, industry-standard stack aligned to this repository: an Angular front end and an ASP.NET Core (.NET 9) backend with a relational database (e.g., PostgreSQL). The system prioritizes:
1.  **Viability:** Core modules (Patient Management, Triage, CPOE) that work out-of-the-box.
2.  **Architecture:** An "Internal-as-External" design using HL7/FHIR messaging or APIs, allowing modules to be swapped or extended without breaking the core.
3.  **Ecosystem Enablement:** Explicitly designed to be serviced and customized by small local tech firms, fostering a support economy.

### Key Differentiators

*   **Architectural Decoupling:** Treats internal modules (Lab, Pharmacy) as external integrations, ensuring future-proof scalability from clinic to hospital networks.
*   **Pragmatic Simplicity:** Focuses on "Clinical Reality" (progressive data capture, dynamic forms) over administrative rigidity.
*   **Economic Model:** Zero license cost + local support economy = sustainable lower total cost of ownership (TCO).

## Target Users

### Primary Users

**1. Dr. Sarah (The Clinician)**
*   **Role:** Doctor / Specialist.
*   **Goal:** Efficiently treat patients with full context (History, Vitals, Labs).
*   **Pain Points:** Fragmented data, slow legacy systems, safety risks from missing info.
*   **Key Interaction:** Reviewing the "Patient Dashboard" (History + Triage Notes), creating Clinical Notes, placing Orders (CPOE).

**2. Alex (The Operational Hybrid)**
*   **Role:** Nurse / Receptionist / Staff.
*   **Goal:** Manage patient flow, rapid registration, and initial triage.
   *   **Context:** Often wears multiple hats in smaller clinics; needs seamless **Role Switching**.
*   **Key Interaction:** "Rapid Registration" (Basic/Full), capturing Vitals/Triage, queuing patients for the doctor.

**3. The Ancillary Team (Lab Tech & Pharmacist)**
*   **Role:** Fulfillment Staff.
*   **Goal:** Receive orders, process them (Test/Dispense), and update status efficiently.
*   **Key Interaction:** Working from a "Task Queue" (e.g., "Pending Lab Orders"), entering results, managing inventory stock.

### Secondary Users

**1. The Patient**
*   **Access:** Separate, secure login portal.
*   **Goal:** Self-registration, viewing history/prescriptions, booking appointments.

**2. The Admin Team (Local Tech Partner)**
*   **Role:** System Administrator / IT Support.
*   **Goal:** Configure the system (Dynamic Forms, Users, Roles), manage backups, and ensure uptime. They need technical documentation and easy configuration tools.

### User Journey (The "Standard Visit" Flow)

1.  **Arrival:** Patient walks in or books online.
2.  **Registration:** Alex (Reception) finds existing record or performs "Basic Registration."
3.  **Triage:** Alex (switching to Nurse role) captures Vitals and Chief Complaint using a Dynamic Assessment Form.
4.  **Consultation:** Dr. Sarah sees the patient in her queue, reviews the holistic summary (History + Triage), documents the visit, and orders a Lab Test.
5.  **Fulfillment:** The Lab Tech sees the new order, performs the test, and enters the result.
6.  **Completion/Billing:** Dr. Sarah reviews the result, finalizes the diagnosis/prescription. The Billing module auto-generates the invoice based on the consultation and test.

## Success Metrics

### User Success Metrics (The "No Dead Ends" Policy)

*   **Zero Critical Blockers:** Users should never be forced to abandon the system during a patient visit.
    *   *Metric:* **System Abandonment Rate** (Frequency of reverting to purely paper workflows outside the system).
*   **Seamless "Gap" Handling:** When a complex automated workflow isn't available (e.g., complex insurance adjudication), the system must effortlessly accept a manual workaround (e.g., scan upload + note) without breaking the data trail.
    *   *Metric:* **Fallback Efficiency** (Time taken to upload a manual override/document < 1 minute).
*   **Operational Speed:** The system must be faster than the paper process it replaces for core tasks.
    *   *Metric:* **Patient Intake Time** (Time from walk-in to triage completion).

### Business Objectives (Project Viability)

*   **Viability & Trust:** Establish the project as a stable, "it just works" solution for small providers.
    *   *Objective:* Achieve a reputation for stability where "Basic" features work flawlessly out-of-the-box.
*   **Ecosystem Activation:** Stimulate a market for local support.
    *   *Objective:* Growing number of local tech firms engaging with the repository or offering managed hosting.
*   **Cost Reduction for Clinics:** Drastically lower the Total Cost of Ownership (TCO) compared to proprietary systems.

### Key Performance Indicators (KPIs)

1.  **Time-to-First-Patient:** < 60 Minutes (From server deploy to registering the first patient).
2.  **System Uptime & Stability:** 99.9% uptime during clinic hours; Zero data loss incidents.
3.  **Manual Fallback Usage:** Monitoring the usage of "Manual Document Uploads" vs. "Structured Data Entry" to identify which features need automation next, while ensuring the manual path remains friction-free.
4.  **Audit Completeness:** 100% of "Manual Overrides" (e.g., invoice changes) must have an associated reason code or attachment log.

## MVP Scope

### Core Features

The Minimum Viable Product (MVP) for the Open Source HMS will focus on delivering a stable, compliant, and efficient core that addresses the most critical day-to-day operations of a small hospital, nursing home, or clinic.

**1. Identity & Access Management (IAM):**
*   **Role-Based Access:** Secure logins for all defined user roles (Doctor, Nurse, Staff, Administrator, Lab Tech, Pharmacist) with granular permissions.
*   **Authentication:** JWT-based authentication for secure inter-module communication.
*   **User Management:** Basic CRUD operations for users and roles.
*   **Role Switching:** For users with multiple assigned roles (e.g., a Nurse who also manages front desk).

**2. Patient Registration & Management:**
*   **Progressive Registration:**
    *   **Basic Registration:** Fast capture of essential patient details (Name, Contact, Consent, Next of Kin, ID) for urgent cases, generating an MRN immediately.
    *   **Staff-Assisted Full Registration:** Staff can complete comprehensive demographic details (Address, Insurance, Payment) after initial care.
    *   **Self-Registration:** Patients can perform basic registration (generating an MRN) via a public interface.
*   **Medical Record Number (MRN):** Unique, life-long identifier for each patient.
*   **Patient Search & Lookup:** Efficient retrieval of patient records.
*   **Basic Patient Demographics:** Store and display essential patient information.

**3. Clinical Core:**
*   **Patient Dashboard/Summary:** A consolidated view for clinicians showing basic history, vitals, current medications, orders, and all completed assessments. Critical data (e.g., abnormal vitals) should be flagged.
*   **Triage Module:**
    *   Capture Current Vitals (Temperature, BP, Pulse, etc.).
    *   Document Chief Complaint/Reason for Visit.
*   **Dynamic Assessment Engine:**
    *   Administrator-level functionality to create and manage custom assessment forms (e.g., CDC-specific forms, pain assessments) with reusable fields.
    *   Clinicians can complete these forms for patients.
*   **Computerized Physician Order Entry (CPOE):**
    *   Doctors/Authorized personnel can place orders for Lab Tests, Medications, Radiology, or other medical services.
    *   Patient summary and alerts visible during ordering process (no auto-ordering).
*   **Clinical Notes:** Capture and manage doctor's and nurse's consultation notes securely.
*   **Inpatient/Outpatient/Day Care Management:** Basic patient flow management (Admit, Discharge, Transfer between departments/units).

**4. Decoupled Support Modules (Internal "MVP" Version):**
These modules will communicate with the core system via the PostgreSQL-based message queue using HL7-based JSON messages.

*   **Lab Module:**
    *   Receive Lab Orders.
    *   Input Lab Results (manual entry for MVP).
    *   Associate results with patient records.
*   **Pharmacy Module:**
    *   Receive Medication Orders.
    *   Dispense Medications (auto-decrements Inventory).
*   **Basic Billing Module:**
    *   Automatically aggregate consultation fees and fixed prices from Lab/Medication orders.
    *   Generate a basic invoice for the patient.
    *   Allow staff to add/remove items with a mandatory audit log (notes/attachments for discrepancies).
*   **Basic Inventory Module:**
    *   Track "Stock on Hand" with pricing for critical items (e.g., medications, basic supplies).
    *   Manual stock updates.

**5. Core Data Standards & Compliance:**
*   **Electronic Health Record (EHR) Foundation:** Secure storage and retrieval of all clinical data.
*   **Data Privacy & Security:** Architectural considerations for HIPAA/GDPR compliance (encryption, access controls, audit trails).
*   **Interoperability:** HL7-based messaging for internal module communication, laying the groundwork for external integrations.

### Out of Scope for MVP

To maintain focus and deliver value quickly, the following will be intentionally excluded from the MVP:

*   **Full Patient Portal:** Advanced features like extensive medical history viewing, online appointment booking, secure messaging with providers, telehealth integration.
*   **Advanced Billing & Revenue Cycle Management (RCM):** Complex insurance adjudication, electronic claims submission, detailed financial reporting, integration with external billing systems.
*   **Advanced Inventory & Supply Chain Management:** Supplier management, purchase order generation, expiry date tracking, lot numbers, cold chain logistics.
*   **Telemedicine Integration:** Direct video consultations, remote patient monitoring platforms (beyond basic vitals capture).
*   **AI/Machine Learning Features:** Predictive analytics, diagnostic support, automated care pathways.
*   **Deep Third-Party Integrations:** Direct, complex integration with external Lab Information Systems (LIS) or Radiology Information Systems (RIS). The internal lightweight modules will suffice.
*   **Automated Public Health Reporting:** Automatic flagging or submission of public health data (e.g., CDC outbreak reports).

### MVP Success Criteria

The success of the MVP will be measured by its ability to deliver a stable, reliable, and functional core system that significantly improves daily operations for small healthcare providers, while building trust and enabling future growth.

*   **Time-to-First-Patient:** < 60 Minutes (From server deploy to registering and triaging the first patient).
*   **System Uptime & Stability:** 99.9% uptime during clinic operating hours; Zero critical data loss incidents.
*   **Zero Workflow Blockers:** Users never forced to abandon the system for paper during a patient interaction workflow.
*   **Fallback Efficiency:** Manual overrides (e.g., invoice adjustments, document uploads for missing features) are seamless and complete in < 1 minute.
*   **Audit Completeness:** 100% of manual overrides or data changes have an associated reason code or audit trail entry.

### Future Vision

Should the MVP prove successful and gain traction, the future vision for the Open Source HMS includes:

*   **Comprehensive Patient Portal:** Enabling patients to manage appointments, access full medical history, and engage in secure communication.
*   **Advanced Analytics & AI/ML:** Implementing predictive diagnostics, workflow optimization, and sophisticated public health surveillance tools.
*   **Robust External System Integration:** Seamless connections with third-party LIS, RIS, Picture Archiving and Communication Systems (PACS), national health registries, and billing systems via FHIR/HL7.
*   **Dedicated Mobile Applications:** Native mobile apps for clinicians for on-the-go patient management and data entry.
*   **Full Financial & Operational Suite:** Comprehensive insurance adjudication, integrated supply chain management, and advanced operational dashboards.
*   **Telehealth & Remote Monitoring:** Integrated virtual consultation capabilities and robust support for remote patient monitoring devices.
*   **Global Expansion & Localization:** Adapting to diverse regulatory environments and linguistic needs worldwide.