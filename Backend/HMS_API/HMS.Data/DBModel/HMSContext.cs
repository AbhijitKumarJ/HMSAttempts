using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.DBModel;

public partial class HMSContext : DbContext
{
    public HMSContext()
    {
    }

    public HMSContext(DbContextOptions<HMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Assessment> Assessments { get; set; }

    public virtual DbSet<AssessmentFieldValue> AssessmentFieldValues { get; set; }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<DynamicForm> DynamicForms { get; set; }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<Field> Fields { get; set; }

    public virtual DbSet<FieldDefinition> FieldDefinitions { get; set; }

    public virtual DbSet<FormField> FormFields { get; set; }

    public virtual DbSet<Lab> Labs { get; set; }

    public virtual DbSet<Medication> Medications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vital> Vitals { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseNpgsql("");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("appointment_status_enum", new[] { "Scheduled", "Completed", "Cancelled" })
            .HasPostgresEnum("billing_status_enum", new[] { "Paid", "Pending", "Cancelled" })
            .HasPostgresEnum("field_data_type_enum", new[] { "Integer", "Decimal", "String", "Date", "Boolean" })
            .HasPostgresEnum("gender_enum", new[] { "Male", "Female", "Other" })
            .HasPostgresEnum("order_status_enum", new[] { "Pending", "Completed", "Cancelled" })
            .HasPostgresEnum("ui_control_type_enum", new[] { "Textbox", "Textarea", "Dropdown", "Radio", "Checkbox", "DatePicker", "NumberSpinner" });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("appointments_pkey");

            entity.ToTable("appointments");

            entity.HasIndex(e => e.PatientId, "appointments_patient_idx");

            entity.Property(e => e.AppointmentId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("appointment_id");
            entity.Property(e => e.AppointmentDate).HasColumnName("appointment_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("appointments_doctor_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("appointments_patient_id_fkey");
        });

        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.HasKey(e => e.AssessmentId).HasName("assessments_pkey");

            entity.ToTable("assessments");

            entity.Property(e => e.AssessmentId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("assessment_id");
            entity.Property(e => e.AssessmentDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("assessment_date");
            entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DynamicFormId).HasColumnName("dynamic_form_id");
            entity.Property(e => e.Results)
                .HasColumnType("jsonb")
                .HasColumnName("results");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Consultation).WithMany(p => p.Assessments)
                .HasForeignKey(d => d.ConsultationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("assessments_consultation_id_fkey");

            entity.HasOne(d => d.DynamicForm).WithMany(p => p.Assessments)
                .HasForeignKey(d => d.DynamicFormId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("assessments_dynamic_form_id_fkey");
        });

        modelBuilder.Entity<AssessmentFieldValue>(entity =>
        {
            entity.HasKey(e => e.AssessmentFieldValueId).HasName("assessment_field_values_pkey");

            entity.ToTable("assessment_field_values");

            entity.HasIndex(e => e.AssessmentId, "afv_assessment_idx");

            entity.Property(e => e.AssessmentFieldValueId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("assessment_field_value_id");
            entity.Property(e => e.AssessmentId).HasColumnName("assessment_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FieldId).HasColumnName("field_id");
            entity.Property(e => e.FormFieldId).HasColumnName("form_field_id");
            entity.Property(e => e.RawValue).HasColumnName("raw_value");
            entity.Property(e => e.RecordedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("recorded_at");
            entity.Property(e => e.RecordedByUserId).HasColumnName("recorded_by_user_id");
            entity.Property(e => e.TypedValue)
                .HasColumnType("jsonb")
                .HasColumnName("typed_value");
            entity.Property(e => e.Unit).HasColumnName("unit");

            entity.HasOne(d => d.Assessment).WithMany(p => p.AssessmentFieldValues)
                .HasForeignKey(d => d.AssessmentId)
                .HasConstraintName("assessment_field_values_assessment_id_fkey");

            entity.HasOne(d => d.Field).WithMany(p => p.AssessmentFieldValues)
                .HasForeignKey(d => d.FieldId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("assessment_field_values_field_id_fkey");

            entity.HasOne(d => d.FormField).WithMany(p => p.AssessmentFieldValues)
                .HasForeignKey(d => d.FormFieldId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("assessment_field_values_form_field_id_fkey");

            entity.HasOne(d => d.RecordedByUser).WithMany(p => p.AssessmentFieldValues)
                .HasForeignKey(d => d.RecordedByUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("assessment_field_values_recorded_by_user_id_fkey");
        });

        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.BillingId).HasName("billing_pkey");

            entity.ToTable("billing");

            entity.Property(e => e.BillingId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("billing_id");
            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Patient).WithMany(p => p.Billings)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("billing_patient_id_fkey");
        });

        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasKey(e => e.ConsultationId).HasName("consultations_pkey");

            entity.ToTable("consultations");

            entity.Property(e => e.ConsultationId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("consultation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.EncounterDate).HasColumnName("encounter_date");
            entity.Property(e => e.EpisodeId).HasColumnName("episode_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("consultations_doctor_id_fkey");

            entity.HasOne(d => d.Episode).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.EpisodeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("consultations_episode_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Consultations)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("consultations_patient_id_fkey");
        });

        modelBuilder.Entity<DynamicForm>(entity =>
        {
            entity.HasKey(e => e.DynamicFormId).HasName("dynamic_forms_pkey");

            entity.ToTable("dynamic_forms");

            entity.Property(e => e.DynamicFormId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("dynamic_form_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Fields)
                .HasColumnType("jsonb")
                .HasColumnName("fields");
            entity.Property(e => e.FormName).HasColumnName("form_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => e.EpisodeId).HasName("episodes_pkey");

            entity.ToTable("episodes");

            entity.Property(e => e.EpisodeId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("episode_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Diagnosis).HasColumnName("diagnosis");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Patient).WithMany(p => p.Episodes)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("episodes_patient_id_fkey");
        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fields_pkey");

            entity.ToTable("fields");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DynamicFormId).HasColumnName("dynamic_form_id");
            entity.Property(e => e.FieldName).HasColumnName("field_name");
            entity.Property(e => e.FieldType).HasColumnName("field_type");
            entity.Property(e => e.IsRequired)
                .HasDefaultValue(false)
                .HasColumnName("is_required");

            entity.HasOne(d => d.DynamicForm).WithMany(p => p.FieldsNavigation)
                .HasForeignKey(d => d.DynamicFormId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fields_dynamic_form_id_fkey");
        });

        modelBuilder.Entity<FieldDefinition>(entity =>
        {
            entity.HasKey(e => e.FieldId).HasName("field_definitions_pkey");

            entity.ToTable("field_definitions");

            entity.HasIndex(e => e.FieldCode, "field_definitions_field_code_key").IsUnique();

            entity.Property(e => e.FieldId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("field_id");
            entity.Property(e => e.AllowedValues)
                .HasColumnType("jsonb")
                .HasColumnName("allowed_values");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.FieldCode).HasColumnName("field_code");
            entity.Property(e => e.FieldName).HasColumnName("field_name");
            entity.Property(e => e.IsRepeatable)
                .HasDefaultValue(false)
                .HasColumnName("is_repeatable");
            entity.Property(e => e.Unit).HasColumnName("unit");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.ValidationRules)
                .HasColumnType("jsonb")
                .HasColumnName("validation_rules");
        });

        modelBuilder.Entity<FormField>(entity =>
        {
            entity.HasKey(e => e.FormFieldId).HasName("form_fields_pkey");

            entity.ToTable("form_fields");

            entity.HasIndex(e => new { e.DynamicFormId, e.FieldId }, "form_fields_dynamic_form_id_field_id_key").IsUnique();

            entity.Property(e => e.FormFieldId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("form_field_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName("display_order");
            entity.Property(e => e.DynamicFormId).HasColumnName("dynamic_form_id");
            entity.Property(e => e.FieldId).HasColumnName("field_id");
            entity.Property(e => e.IsRequiredOverride).HasColumnName("is_required_override");
            entity.Property(e => e.LabelOverride).HasColumnName("label_override");
            entity.Property(e => e.UiOptions)
                .HasColumnType("jsonb")
                .HasColumnName("ui_options");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.VisibilityCondition)
                .HasColumnType("jsonb")
                .HasColumnName("visibility_condition");

            entity.HasOne(d => d.DynamicForm).WithMany(p => p.FormFields)
                .HasForeignKey(d => d.DynamicFormId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("form_fields_dynamic_form_id_fkey");

            entity.HasOne(d => d.Field).WithMany(p => p.FormFields)
                .HasForeignKey(d => d.FieldId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("form_fields_field_id_fkey");
        });

        modelBuilder.Entity<Lab>(entity =>
        {
            entity.HasKey(e => e.LabId).HasName("labs_pkey");

            entity.ToTable("labs");

            entity.Property(e => e.LabId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("lab_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LabName).HasColumnName("lab_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.MedicationId).HasName("medications_pkey");

            entity.ToTable("medications");

            entity.Property(e => e.MedicationId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("medication_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.Dosage).HasColumnName("dosage");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.MedicationName).HasColumnName("medication_name");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Patient).WithMany(p => p.Medications)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("medications_patient_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.OrderId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("order_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.LabId).HasColumnName("lab_id");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("now()")
                .HasColumnName("order_date");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Lab).WithMany(p => p.Orders)
                .HasForeignKey(d => d.LabId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("orders_lab_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("orders_patient_id_fkey");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("patients_pkey");

            entity.ToTable("patients");

            entity.HasIndex(e => e.Mrn, "patients_mrn_idx");

            entity.HasIndex(e => e.Mrn, "patients_mrn_key").IsUnique();

            entity.Property(e => e.PatientId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("patient_id");
            entity.Property(e => e.ContactInfo)
                .HasColumnType("jsonb")
                .HasColumnName("contact_info");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Mrn).HasColumnName("mrn");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "roles_role_name_key").IsUnique();

            entity.Property(e => e.RoleId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("role_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "users_role_id_idx");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.UserId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username).HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<Vital>(entity =>
        {
            entity.HasKey(e => e.VitalId).HasName("vitals_pkey");

            entity.ToTable("vitals");

            entity.HasIndex(e => e.PatientId, "vitals_patient_idx");

            entity.Property(e => e.VitalId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("vital_id");
            entity.Property(e => e.BloodPressure).HasColumnName("blood_pressure");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("created_at");
            entity.Property(e => e.HeartRate).HasColumnName("heart_rate");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.RecordedAt)
                .HasDefaultValueSql("now()")
                .HasColumnName("recorded_at");
            entity.Property(e => e.Temperature).HasColumnName("temperature");

            entity.HasOne(d => d.Patient).WithMany(p => p.Vitals)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("vitals_patient_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
