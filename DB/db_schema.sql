-- PostgreSQL schema for the HMS system
-- Generated from DB/db_design.md

-- Drop existing tables/types to ensure clean slate
DROP SCHEMA IF EXISTS public CASCADE;
CREATE SCHEMA public;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO public;

-- Helper: set updated_at on update
CREATE OR REPLACE FUNCTION hms_set_timestamp()
RETURNS TRIGGER AS $$
BEGIN
	NEW.updated_at = now();
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- 1. Core Infrastructure & Config

-- app_events
-- Used for the "Internal-as-External" async communication pattern.
CREATE TABLE app_events (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    event_type VARCHAR(150) NOT NULL,
    payload JSONB,
    status VARCHAR(50) DEFAULT 'Pending', -- 'Pending', 'Processing', 'Completed', 'Failed'
    created_at TIMESTAMP DEFAULT NOW(),
    processed_at TIMESTAMP,
    failure_count INT DEFAULT 0
);
CREATE INDEX idx_app_events_event_type ON app_events(event_type);

-- sys_config
-- Global key-value settings.
CREATE TABLE sys_config (
    key VARCHAR(100) PRIMARY KEY,
    value TEXT,
    description TEXT,
    updated_at TIMESTAMP
);
CREATE TRIGGER sys_config_set_timestamp BEFORE UPDATE ON sys_config
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- audit_logs
-- Immutable history of critical actions.
CREATE TABLE audit_logs (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    entity_type VARCHAR(100),
    entity_id VARCHAR(100),
    action VARCHAR(50), -- 'Update', 'Delete', 'Dispense'
    old_value JSONB,
    new_value JSONB,
    reason TEXT,
    user_id INT,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 2. Identity & Access

-- users
CREATE TABLE users (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    password_hash VARCHAR(255),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT NOW()
);

-- roles
CREATE TABLE roles (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(50) NOT NULL -- 'Doctor', 'Nurse', 'Admin', 'Receptionist'
);

-- user_roles
CREATE TABLE user_roles (
    user_id INT REFERENCES users(id) ON DELETE CASCADE,
    role_id INT REFERENCES roles(id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);

-- 3. Scheduling & Patient Management

-- pat_patients
CREATE TABLE pat_patients (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    mrn VARCHAR(20) UNIQUE NOT NULL,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    gender VARCHAR(20),
    dob DATE,
    contact_info JSONB,
    is_emergency_reg BOOLEAN,
    created_at TIMESTAMP DEFAULT NOW()
);
CREATE INDEX idx_pat_patients_mrn ON pat_patients(mrn);

-- sch_appointments
CREATE TABLE sch_appointments (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    patient_id INT REFERENCES pat_patients(id) ON DELETE CASCADE,
    doctor_id INT REFERENCES users(id) ON DELETE SET NULL,
    appointment_date TIMESTAMP,
    status VARCHAR(50) DEFAULT 'Scheduled', -- 'Scheduled', 'CheckedIn', 'Completed', 'Cancelled', 'NoShow'
    reason_for_visit TEXT,
    created_at TIMESTAMP DEFAULT NOW()
);

-- sch_episodes
-- Groups related encounters (e.g., "Pregnancy 2024", "Broken Leg").
CREATE TABLE sch_episodes (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    patient_id INT REFERENCES pat_patients(id) ON DELETE CASCADE,
    title VARCHAR(200),
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    status VARCHAR(50) -- 'Active', 'Resolved'
);

-- 4. Clinical Core & Dynamic Forms

-- clin_consultations
-- The central event of a clinical interaction.
CREATE TABLE clin_consultations (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    appointment_id BIGINT REFERENCES sch_appointments(id) ON DELETE SET NULL,
    episode_id BIGINT REFERENCES sch_episodes(id) ON DELETE SET NULL,
    patient_id INT REFERENCES pat_patients(id) ON DELETE CASCADE,
    doctor_id INT REFERENCES users(id) ON DELETE SET NULL,
    started_at TIMESTAMP,
    ended_at TIMESTAMP,
    clinical_summary TEXT
);

-- clin_vitals
-- Standardized high-frequency data.
CREATE TABLE clin_vitals (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    patient_id INT REFERENCES pat_patients(id) ON DELETE CASCADE,
    consultation_id BIGINT REFERENCES clin_consultations(id) ON DELETE SET NULL,
    bp_systolic INT,
    bp_diastolic INT,
    heart_rate INT,
    temperature DECIMAL(4,1),
    spo2 INT,
    recorded_at TIMESTAMP DEFAULT NOW(),
    recorded_by INT REFERENCES users(id) ON DELETE SET NULL
);

-- clin_field_definitions
-- Library of reusable clinical data points (e.g., "Smoking Status", "Pain Score").
CREATE TABLE clin_field_definitions (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    field_code VARCHAR(50) UNIQUE NOT NULL,
    name VARCHAR(100),
    data_type VARCHAR(20), -- 'Integer', 'String', 'Boolean', 'Date', 'Option'
    unit VARCHAR(20),
    validation_rules JSONB,
    options JSONB
);

-- clin_form_templates
-- Definitions of forms (e.g., "Triage Form v1").
CREATE TABLE clin_form_templates (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    title VARCHAR(200),
    is_active BOOLEAN DEFAULT TRUE,
    version INT DEFAULT 1
);

-- clin_form_fields
-- Many-to-Many mapping specific fields to templates with overrides.
CREATE TABLE clin_form_fields (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    template_id INT REFERENCES clin_form_templates(id) ON DELETE CASCADE,
    field_id INT REFERENCES clin_field_definitions(id) ON DELETE CASCADE,
    label_override VARCHAR(100),
    display_order INT,
    is_required BOOLEAN DEFAULT FALSE,
    ui_control VARCHAR(50) -- 'Textbox', 'Dropdown', 'Radio'
);

-- clin_assessments
-- An instance of a filled-out form.
CREATE TABLE clin_assessments (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    patient_id INT REFERENCES pat_patients(id) ON DELETE CASCADE,
    consultation_id BIGINT REFERENCES clin_consultations(id) ON DELETE SET NULL,
    template_id INT REFERENCES clin_form_templates(id) ON DELETE SET NULL,
    performed_at TIMESTAMP DEFAULT NOW(),
    performed_by INT REFERENCES users(id) ON DELETE SET NULL
);

-- clin_assessment_values
-- The actual data captured.
CREATE TABLE clin_assessment_values (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    assessment_id BIGINT REFERENCES clin_assessments(id) ON DELETE CASCADE,
    field_id INT REFERENCES clin_field_definitions(id) ON DELETE CASCADE,
    value_raw TEXT,
    value_typed JSONB,
    created_at TIMESTAMP DEFAULT NOW()
);

-- clin_macros
CREATE TABLE clin_macros (
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    trigger_key VARCHAR(50),
    expansion TEXT,
    user_id INT REFERENCES users(id) ON DELETE CASCADE
);

-- 5. Orders & Labs

-- ord_orders
CREATE TABLE ord_orders (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    patient_id INT REFERENCES pat_patients(id) ON DELETE CASCADE,
    consultation_id BIGINT REFERENCES clin_consultations(id) ON DELETE SET NULL,
    type VARCHAR(50), -- 'Lab', 'Medication'
    description TEXT,
    priority VARCHAR(20), -- 'Routine', 'Stat'
    status VARCHAR(50),
    ordered_by INT REFERENCES users(id) ON DELETE SET NULL,
    created_at TIMESTAMP DEFAULT NOW()
);

-- lab_results
CREATE TABLE lab_results (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    order_id BIGINT REFERENCES ord_orders(id) ON DELETE CASCADE,
    result_summary TEXT,
    result_data JSONB,
    released_at TIMESTAMP
);

-- 6. Inventory & Pharmacy

-- inv_items
CREATE TABLE inv_items (
    sku VARCHAR(50) PRIMARY KEY,
    name VARCHAR(200),
    quantity INT DEFAULT 0,
    min_reorder_level INT DEFAULT 0,
    unit_price DECIMAL(10,2),
    updated_at TIMESTAMP
);
CREATE TRIGGER inv_items_set_timestamp BEFORE UPDATE ON inv_items
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- inv_transactions
CREATE TABLE inv_transactions (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    sku VARCHAR(50) REFERENCES inv_items(sku) ON DELETE CASCADE,
    change_amount INT,
    reason VARCHAR(200),
    user_id INT REFERENCES users(id) ON DELETE SET NULL,
    created_at TIMESTAMP DEFAULT NOW()
);

-- 7. Billing

-- bil_invoices
CREATE TABLE bil_invoices (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    patient_id INT REFERENCES pat_patients(id) ON DELETE CASCADE,
    total_amount DECIMAL(12,2),
    status VARCHAR(50), -- 'Open', 'Paid', 'Void'
    created_at TIMESTAMP DEFAULT NOW()
);

-- bil_invoice_items
CREATE TABLE bil_invoice_items (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    invoice_id BIGINT REFERENCES bil_invoices(id) ON DELETE CASCADE,
    description VARCHAR(200),
    quantity INT,
    unit_price DECIMAL(10,2),
    total_price DECIMAL(10,2),
    source_event_id UUID
);