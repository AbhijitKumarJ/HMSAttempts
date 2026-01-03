-- PostgreSQL schema for the HMS system
-- Generated from DB/db_design.md

-- Drop existing types/tables if needed (safe on dev, be careful on prod)
SET client_min_messages = WARNING;

-- Enum types
DO $$ BEGIN
	IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'gender_enum') THEN
		CREATE TYPE gender_enum AS ENUM ('Male','Female','Other');
	END IF;
	IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'appointment_status_enum') THEN
		CREATE TYPE appointment_status_enum AS ENUM ('Scheduled','Completed','Cancelled');
	END IF;
	IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'order_status_enum') THEN
		CREATE TYPE order_status_enum AS ENUM ('Pending','Completed','Cancelled');
	END IF;
	IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'billing_status_enum') THEN
		CREATE TYPE billing_status_enum AS ENUM ('Paid','Pending','Cancelled');
	END IF;
	IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'field_data_type_enum') THEN
		CREATE TYPE field_data_type_enum AS ENUM ('Integer','Decimal','String','Date','Boolean');
	END IF;
	IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'ui_control_type_enum') THEN
		CREATE TYPE ui_control_type_enum AS ENUM ('Textbox','Textarea','Dropdown','Radio','Checkbox','DatePicker','NumberSpinner');
	END IF;
END$$;

-- Helper: set updated_at on update
CREATE OR REPLACE FUNCTION hms_set_timestamp()
RETURNS TRIGGER AS $$
BEGIN
	NEW.updated_at = now();
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Roles
CREATE TABLE IF NOT EXISTS roles (
	role_id         BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	role_name       TEXT NOT NULL UNIQUE,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER roles_set_timestamp BEFORE UPDATE ON roles
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Users
CREATE TABLE IF NOT EXISTS users (
	user_id         BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	username        TEXT NOT NULL UNIQUE,
	password_hash   TEXT NOT NULL,
	role_id         BIGINT REFERENCES roles(role_id) ON DELETE SET NULL,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE INDEX IF NOT EXISTS users_role_id_idx ON users(role_id);
CREATE TRIGGER users_set_timestamp BEFORE UPDATE ON users
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Patients
CREATE TABLE IF NOT EXISTS patients (
	patient_id      BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	mrn             TEXT NOT NULL UNIQUE,
	first_name      TEXT,
	last_name       TEXT,
	date_of_birth   DATE,
	gender          gender_enum,
	contact_info    JSONB,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE INDEX IF NOT EXISTS patients_mrn_idx ON patients(mrn);
CREATE TRIGGER patients_set_timestamp BEFORE UPDATE ON patients
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Appointments
CREATE TABLE IF NOT EXISTS appointments (
	appointment_id  BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	patient_id      BIGINT NOT NULL REFERENCES patients(patient_id) ON DELETE CASCADE,
	doctor_id       BIGINT REFERENCES users(user_id) ON DELETE SET NULL,
	appointment_date TIMESTAMP WITH TIME ZONE,
	status          appointment_status_enum DEFAULT 'Scheduled',
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE INDEX IF NOT EXISTS appointments_patient_idx ON appointments(patient_id);
CREATE TRIGGER appointments_set_timestamp BEFORE UPDATE ON appointments
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Episodes
CREATE TABLE IF NOT EXISTS episodes (
	episode_id      BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	patient_id      BIGINT NOT NULL REFERENCES patients(patient_id) ON DELETE CASCADE,
	start_date      TIMESTAMP WITH TIME ZONE,
	end_date        TIMESTAMP WITH TIME ZONE,
	diagnosis       TEXT,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER episodes_set_timestamp BEFORE UPDATE ON episodes
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Consultations
CREATE TABLE IF NOT EXISTS consultations (
	consultation_id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	patient_id      BIGINT NOT NULL REFERENCES patients(patient_id) ON DELETE CASCADE,
	doctor_id       BIGINT REFERENCES users(user_id) ON DELETE SET NULL,
	episode_id      BIGINT REFERENCES episodes(episode_id) ON DELETE SET NULL,
	encounter_date  TIMESTAMP WITH TIME ZONE,
	notes           TEXT,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER consultations_set_timestamp BEFORE UPDATE ON consultations
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Dynamic forms and form metadata
CREATE TABLE IF NOT EXISTS dynamic_forms (
	dynamic_form_id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	form_name       TEXT NOT NULL,
	fields          JSONB,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER dynamic_forms_set_timestamp BEFORE UPDATE ON dynamic_forms
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Field definitions (semantic definitions reused across forms)
CREATE TABLE IF NOT EXISTS field_definitions (
	field_id        BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	field_code      TEXT NOT NULL UNIQUE,
	field_name      TEXT,
	data_type       field_data_type_enum NOT NULL,
	unit            TEXT,
	allowed_values  JSONB,
	validation_rules JSONB,
	is_repeatable   BOOLEAN DEFAULT FALSE,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER field_definitions_set_timestamp BEFORE UPDATE ON field_definitions
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Form fields mapping (per-form overrides and UI hints)
CREATE TABLE IF NOT EXISTS form_fields (
	form_field_id   BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	dynamic_form_id BIGINT REFERENCES dynamic_forms(dynamic_form_id) ON DELETE CASCADE,
	field_id        BIGINT REFERENCES field_definitions(field_id) ON DELETE CASCADE,
	label_override  TEXT,
	display_order   INT DEFAULT 0,
	is_required_override BOOLEAN,
	visibility_condition JSONB,
	ui_control_type ui_control_type_enum DEFAULT 'Textbox',
	ui_options      JSONB,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	UNIQUE(dynamic_form_id, field_id)
);
CREATE TRIGGER form_fields_set_timestamp BEFORE UPDATE ON form_fields
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Assessments and their captured field values
CREATE TABLE IF NOT EXISTS assessments (
	assessment_id   BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	consultation_id BIGINT REFERENCES consultations(consultation_id) ON DELETE SET NULL,
	dynamic_form_id BIGINT REFERENCES dynamic_forms(dynamic_form_id) ON DELETE SET NULL,
	assessment_date TIMESTAMP WITH TIME ZONE DEFAULT now(),
	summary         TEXT,
	results         JSONB,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER assessments_set_timestamp BEFORE UPDATE ON assessments
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

CREATE TABLE IF NOT EXISTS assessment_field_values (
	assessment_field_value_id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	assessment_id   BIGINT NOT NULL REFERENCES assessments(assessment_id) ON DELETE CASCADE,
	field_id        BIGINT REFERENCES field_definitions(field_id) ON DELETE SET NULL,
	form_field_id   BIGINT REFERENCES form_fields(form_field_id) ON DELETE SET NULL,
	raw_value       TEXT,
	typed_value     JSONB,
	unit            TEXT,
	recorded_by_user_id BIGINT REFERENCES users(user_id) ON DELETE SET NULL,
	recorded_at     TIMESTAMP WITH TIME ZONE DEFAULT now(),
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE INDEX IF NOT EXISTS afv_assessment_idx ON assessment_field_values(assessment_id);

-- Vitals
CREATE TABLE IF NOT EXISTS vitals (
	vital_id        BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	patient_id      BIGINT NOT NULL REFERENCES patients(patient_id) ON DELETE CASCADE,
	blood_pressure  TEXT,
	heart_rate      INT,
	temperature     DOUBLE PRECISION,
	recorded_at     TIMESTAMP WITH TIME ZONE DEFAULT now(),
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE INDEX IF NOT EXISTS vitals_patient_idx ON vitals(patient_id);

-- Medications
CREATE TABLE IF NOT EXISTS medications (
	medication_id   BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	patient_id      BIGINT NOT NULL REFERENCES patients(patient_id) ON DELETE CASCADE,
	medication_name TEXT,
	dosage          TEXT,
	start_date      TIMESTAMP WITH TIME ZONE,
	end_date        TIMESTAMP WITH TIME ZONE,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER medications_set_timestamp BEFORE UPDATE ON medications
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Labs and Orders
CREATE TABLE IF NOT EXISTS labs (
	lab_id          BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	lab_name        TEXT NOT NULL,
	description     TEXT,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER labs_set_timestamp BEFORE UPDATE ON labs
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

CREATE TABLE IF NOT EXISTS orders (
	order_id        BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	patient_id      BIGINT NOT NULL REFERENCES patients(patient_id) ON DELETE CASCADE,
	lab_id          BIGINT REFERENCES labs(lab_id) ON DELETE SET NULL,
	order_date      TIMESTAMP WITH TIME ZONE DEFAULT now(),
	status          order_status_enum DEFAULT 'Pending',
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER orders_set_timestamp BEFORE UPDATE ON orders
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Billing
CREATE TABLE IF NOT EXISTS billing (
	billing_id      BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	patient_id      BIGINT NOT NULL REFERENCES patients(patient_id) ON DELETE CASCADE,
	amount          NUMERIC(12,2) NOT NULL DEFAULT 0.00,
	status          billing_status_enum DEFAULT 'Pending',
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL,
	updated_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);
CREATE TRIGGER billing_set_timestamp BEFORE UPDATE ON billing
	FOR EACH ROW EXECUTE FUNCTION hms_set_timestamp();

-- Additional metadata for fields (optional "Fields" table in design)
CREATE TABLE IF NOT EXISTS fields (
	id              BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	dynamic_form_id BIGINT REFERENCES dynamic_forms(dynamic_form_id) ON DELETE CASCADE,
	field_name      TEXT,
	field_type      TEXT,
	is_required     BOOLEAN DEFAULT FALSE,
	created_at      TIMESTAMP WITH TIME ZONE DEFAULT now() NOT NULL
);

-- Useful views or indexes can be added here as needed.

-- End of schema

SET client_min_messages = NOTICE;
test