# HMS Database Design

**Conventions:**
*   **Case:** `snake_case` for Tables and Columns (Postgres standard).
*   **Primary Keys:** `id` (Identity/Serial or UUID).
*   **Foreign Keys:** `[entity]_id`.
*   **JSON:** used for flexible configurations and schema definitions (`jsonb`).
*   **Audit:** Most mutable tables have `created_at`, `updated_at`, and `created_by`.

---

## 1. Core Infrastructure & Config

### `app_events` (Message Bus)
Used for the "Internal-as-External" async communication pattern.
*   `id` (UUID, PK)
*   `event_type` (VARCHAR(150), Indexed)
*   `payload` (JSONB)
*   `status` (VARCHAR(50)) -- 'Pending', 'Processing', 'Completed', 'Failed'
*   `created_at` (TIMESTAMP DEFAULT NOW())
*   `processed_at` (TIMESTAMP NULL)
*   `failure_count` (INT DEFAULT 0)

### `sys_config` (System Configuration)
Global key-value settings.
*   `key` (VARCHAR(100), PK)
*   `value` (TEXT)
*   `description` (TEXT)
*   `updated_at` (TIMESTAMP)

### `audit_logs` (Compliance)
Immutable history of critical actions.
*   `id` (BIGSERIAL, PK)
*   `entity_type` (VARCHAR(100)) -- e.g., 'Invoice', 'Patient'
*   `entity_id` (VARCHAR(100))
*   `action` (VARCHAR(50)) -- 'Update', 'Delete', 'Dispense'
*   `old_value` (JSONB NULL)
*   `new_value` (JSONB NULL)
*   `reason` (TEXT NULL)
*   `user_id` (INT)
*   `created_at` (TIMESTAMP DEFAULT NOW())

---

## 2. Identity & Access

### `users`
*   `id` (SERIAL, PK)
*   `username` (VARCHAR(100), Unique)
*   `password_hash` (VARCHAR(255))
*   `is_active` (BOOLEAN DEFAULT TRUE)
*   `created_at` (TIMESTAMP)

### `roles`
*   `id` (SERIAL, PK)
*   `name` (VARCHAR(50)) -- 'Doctor', 'Nurse', 'Admin', 'Receptionist'

### `user_roles`
*   `user_id` (INT, FK -> users.id)
*   `role_id` (INT, FK -> roles.id)
*   (PK: user_id, role_id)

---

## 3. Scheduling & Patient Management

### `pat_patients`
*   `id` (SERIAL, PK)
*   `mrn` (VARCHAR(20), Unique Index)
*   `first_name` (VARCHAR(100))
*   `last_name` (VARCHAR(100))
*   `gender` (VARCHAR(20))
*   `dob` (DATE)
*   `contact_info` (JSONB)
*   `is_emergency_reg` (BOOLEAN)
*   `created_at` (TIMESTAMP)

### `sch_appointments`
*   `id` (BIGSERIAL, PK)
*   `patient_id` (INT, FK -> pat_patients.id)
*   `doctor_id` (INT, FK -> users.id)
*   `appointment_date` (TIMESTAMP)
*   `status` (VARCHAR(50)) -- 'Scheduled', 'CheckedIn', 'Completed', 'Cancelled', 'NoShow'
*   `reason_for_visit` (TEXT)
*   `created_at` (TIMESTAMP)

### `sch_episodes`
Groups related encounters (e.g., "Pregnancy 2024", "Broken Leg").
*   `id` (BIGSERIAL, PK)
*   `patient_id` (INT, FK -> pat_patients.id)
*   `title` (VARCHAR(200))
*   `start_date` (TIMESTAMP)
*   `end_date` (TIMESTAMP NULL)
*   `status` (VARCHAR(50)) -- 'Active', 'Resolved'

---

## 4. Clinical Core & Dynamic Forms

### `clin_consultations`
The central event of a clinical interaction.
*   `id` (BIGSERIAL, PK)
*   `appointment_id` (BIGINT, FK -> sch_appointments.id, Nullable)
*   `episode_id` (BIGINT, FK -> sch_episodes.id, Nullable)
*   `patient_id` (INT, FK -> pat_patients.id)
*   `doctor_id` (INT, FK -> users.id)
*   `started_at` (TIMESTAMP)
*   `ended_at` (TIMESTAMP NULL)
*   `clinical_summary` (TEXT)

### `clin_vitals`
Standardized high-frequency data.
*   `id` (BIGSERIAL, PK)
*   `patient_id` (INT, FK -> pat_patients.id)
*   `consultation_id` (BIGINT, FK -> clin_consultations.id, Nullable)
*   `bp_systolic` (INT)
*   `bp_diastolic` (INT)
*   `heart_rate` (INT)
*   `temperature` (DECIMAL(4,1))
*   `spo2` (INT)
*   `recorded_at` (TIMESTAMP)
*   `recorded_by` (INT, FK -> users.id)

### `clin_field_definitions`
Library of reusable clinical data points (e.g., "Smoking Status", "Pain Score").
*   `id` (SERIAL, PK)
*   `field_code` (VARCHAR(50), Unique) -- e.g., 'pain_score'
*   `name` (VARCHAR(100))
*   `data_type` (VARCHAR(20)) -- 'Integer', 'String', 'Boolean', 'Date', 'Option'
*   `unit` (VARCHAR(20)) -- e.g., 'kg', 'cm'
*   `validation_rules` (JSONB) -- Min/Max, Regex
*   `options` (JSONB) -- For 'Option' types (e.g., ["Smoker", "Non-Smoker"])

### `clin_form_templates`
Definitions of forms (e.g., "Triage Form v1").
*   `id` (SERIAL, PK)
*   `title` (VARCHAR(200))
*   `is_active` (BOOLEAN)
*   `version` (INT)

### `clin_form_fields`
Many-to-Many mapping specific fields to templates with overrides.
*   `id` (SERIAL, PK)
*   `template_id` (INT, FK -> clin_form_templates.id)
*   `field_id` (INT, FK -> clin_field_definitions.id)
*   `label_override` (VARCHAR(100))
*   `display_order` (INT)
*   `is_required` (BOOLEAN)
*   `ui_control` (VARCHAR(50)) -- 'Textbox', 'Dropdown', 'Radio'

### `clin_assessments`
An instance of a filled-out form.
*   `id` (BIGSERIAL, PK)
*   `patient_id` (INT, FK -> pat_patients.id)
*   `consultation_id` (BIGINT, FK -> clin_consultations.id)
*   `template_id` (INT, FK -> clin_form_templates.id)
*   `performed_at` (TIMESTAMP)
*   `performed_by` (INT, FK -> users.id)

### `clin_assessment_values`
The actual data captured.
*   `id` (BIGSERIAL, PK)
*   `assessment_id` (BIGINT, FK -> clin_assessments.id)
*   `field_id` (INT, FK -> clin_field_definitions.id)
*   `value_raw` (TEXT)
*   `value_typed` (JSONB) -- Canonical storage
*   `created_at` (TIMESTAMP)

### `clin_macros`
*   `id` (SERIAL, PK)
*   `trigger_key` (VARCHAR(50))
*   `expansion` (TEXT)
*   `user_id` (INT NULL)

---

## 5. Orders & Labs

### `ord_orders`
*   `id` (BIGSERIAL, PK)
*   `patient_id` (INT, FK -> pat_patients.id)
*   `consultation_id` (BIGINT, FK -> clin_consultations.id, Nullable)
*   `type` (VARCHAR(50)) -- 'Lab', 'Medication'
*   `description` (TEXT)
*   `priority` (VARCHAR(20)) -- 'Routine', 'Stat'
*   `status` (VARCHAR(50))
*   `ordered_by` (INT, FK -> users.id)
*   `created_at` (TIMESTAMP)

### `lab_results`
*   `id` (BIGSERIAL, PK)
*   `order_id` (BIGINT, FK -> ord_orders.id)
*   `result_summary` (TEXT)
*   `result_data` (JSONB) -- Detailed findings
*   `released_at` (TIMESTAMP)

---

## 6. Inventory & Pharmacy

### `inv_items`
*   `sku` (VARCHAR(50), PK)
*   `name` (VARCHAR(200))
*   `quantity` (INT)
*   `min_reorder_level` (INT)
*   `unit_price` (DECIMAL(10,2))
*   `updated_at` (TIMESTAMP)

### `inv_transactions`
*   `id` (BIGSERIAL, PK)
*   `sku` (VARCHAR(50), FK -> inv_items.sku)
*   `change_amount` (INT)
*   `reason` (VARCHAR(200))
*   `user_id` (INT)
*   `created_at` (TIMESTAMP)

---

## 7. Billing

### `bil_invoices`
*   `id` (BIGSERIAL, PK)
*   `patient_id` (INT, FK -> pat_patients.id)
*   `total_amount` (DECIMAL(12,2))
*   `status` (VARCHAR(50)) -- 'Open', 'Paid', 'Void'
*   `created_at` (TIMESTAMP)

### `bil_invoice_items`
*   `id` (BIGSERIAL, PK)
*   `invoice_id` (BIGINT, FK -> bil_invoices.id)
*   `description` (VARCHAR(200))
*   `quantity` (INT)
*   `unit_price` (DECIMAL(10,2))
*   `total_price` (DECIMAL(10,2))
*   `source_event_id` (UUID NULL)