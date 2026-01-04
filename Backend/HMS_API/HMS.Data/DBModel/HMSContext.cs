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

    public virtual DbSet<AppEvent> AppEvents { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<BilInvoice> BilInvoices { get; set; }

    public virtual DbSet<BilInvoiceItem> BilInvoiceItems { get; set; }

    public virtual DbSet<ClinAssessment> ClinAssessments { get; set; }

    public virtual DbSet<ClinAssessmentValue> ClinAssessmentValues { get; set; }

    public virtual DbSet<ClinConsultation> ClinConsultations { get; set; }

    public virtual DbSet<ClinFieldDefinition> ClinFieldDefinitions { get; set; }

    public virtual DbSet<ClinFormField> ClinFormFields { get; set; }

    public virtual DbSet<ClinFormTemplate> ClinFormTemplates { get; set; }

    public virtual DbSet<ClinMacro> ClinMacros { get; set; }

    public virtual DbSet<ClinVital> ClinVitals { get; set; }

    public virtual DbSet<InvItem> InvItems { get; set; }

    public virtual DbSet<InvTransaction> InvTransactions { get; set; }

    public virtual DbSet<LabResult> LabResults { get; set; }

    public virtual DbSet<OrdOrder> OrdOrders { get; set; }

    public virtual DbSet<PatPatient> PatPatients { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SchAppointment> SchAppointments { get; set; }

    public virtual DbSet<SchEpisode> SchEpisodes { get; set; }

    public virtual DbSet<SysConfig> SysConfigs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseNpgsql("");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Add OnModelCreating configuration for RefreshToken entity.
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("refresh_tokens_pkey");

            entity.ToTable("refresh_tokens");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.TokenHash)
                .HasMaxLength(255)
                .HasColumnName("token_hash");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("refresh_tokens_user_id_fkey");
        });
        

        modelBuilder.Entity<AppEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("app_events_pkey");

            entity.ToTable("app_events");

            entity.HasIndex(e => e.EventType, "idx_app_events_event_type");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EventType)
                .HasMaxLength(150)
                .HasColumnName("event_type");
            entity.Property(e => e.FailureCount)
                .HasDefaultValue(0)
                .HasColumnName("failure_count");
            entity.Property(e => e.Payload)
                .HasColumnType("jsonb")
                .HasColumnName("payload");
            entity.Property(e => e.ProcessedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("processed_at");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("audit_logs_pkey");

            entity.ToTable("audit_logs");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EntityId)
                .HasMaxLength(100)
                .HasColumnName("entity_id");
            entity.Property(e => e.EntityType)
                .HasMaxLength(100)
                .HasColumnName("entity_type");
            entity.Property(e => e.NewValue)
                .HasColumnType("jsonb")
                .HasColumnName("new_value");
            entity.Property(e => e.OldValue)
                .HasColumnType("jsonb")
                .HasColumnName("old_value");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<BilInvoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bil_invoices_pkey");

            entity.ToTable("bil_invoices");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(12, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Patient).WithMany(p => p.BilInvoices)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bil_invoices_patient_id_fkey");
        });

        modelBuilder.Entity<BilInvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bil_invoice_items_pkey");

            entity.ToTable("bil_invoice_items");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SourceEventId).HasColumnName("source_event_id");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(10, 2)
                .HasColumnName("total_price");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Invoice).WithMany(p => p.BilInvoiceItems)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bil_invoice_items_invoice_id_fkey");
        });

        modelBuilder.Entity<ClinAssessment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clin_assessments_pkey");

            entity.ToTable("clin_assessments");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.PerformedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("performed_at");
            entity.Property(e => e.PerformedBy).HasColumnName("performed_by");
            entity.Property(e => e.TemplateId).HasColumnName("template_id");

            entity.HasOne(d => d.Consultation).WithMany(p => p.ClinAssessments)
                .HasForeignKey(d => d.ConsultationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("clin_assessments_consultation_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.ClinAssessments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("clin_assessments_patient_id_fkey");

            entity.HasOne(d => d.PerformedByNavigation).WithMany(p => p.ClinAssessments)
                .HasForeignKey(d => d.PerformedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("clin_assessments_performed_by_fkey");

            entity.HasOne(d => d.Template).WithMany(p => p.ClinAssessments)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("clin_assessments_template_id_fkey");
        });

        modelBuilder.Entity<ClinAssessmentValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clin_assessment_values_pkey");

            entity.ToTable("clin_assessment_values");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AssessmentId).HasColumnName("assessment_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FieldId).HasColumnName("field_id");
            entity.Property(e => e.ValueRaw).HasColumnName("value_raw");
            entity.Property(e => e.ValueTyped)
                .HasColumnType("jsonb")
                .HasColumnName("value_typed");

            entity.HasOne(d => d.Assessment).WithMany(p => p.ClinAssessmentValues)
                .HasForeignKey(d => d.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("clin_assessment_values_assessment_id_fkey");

            entity.HasOne(d => d.Field).WithMany(p => p.ClinAssessmentValues)
                .HasForeignKey(d => d.FieldId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("clin_assessment_values_field_id_fkey");
        });

        modelBuilder.Entity<ClinConsultation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clin_consultations_pkey");

            entity.ToTable("clin_consultations");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.ClinicalSummary).HasColumnName("clinical_summary");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.EndedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("ended_at");
            entity.Property(e => e.EpisodeId).HasColumnName("episode_id");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.StartedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("started_at");

            entity.HasOne(d => d.Appointment).WithMany(p => p.ClinConsultations)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("clin_consultations_appointment_id_fkey");

            entity.HasOne(d => d.Doctor).WithMany(p => p.ClinConsultations)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("clin_consultations_doctor_id_fkey");

            entity.HasOne(d => d.Episode).WithMany(p => p.ClinConsultations)
                .HasForeignKey(d => d.EpisodeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("clin_consultations_episode_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.ClinConsultations)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("clin_consultations_patient_id_fkey");
        });

        modelBuilder.Entity<ClinFieldDefinition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clin_field_definitions_pkey");

            entity.ToTable("clin_field_definitions");

            entity.HasIndex(e => e.FieldCode, "clin_field_definitions_field_code_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DataType)
                .HasMaxLength(20)
                .HasColumnName("data_type");
            entity.Property(e => e.FieldCode)
                .HasMaxLength(50)
                .HasColumnName("field_code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Options)
                .HasColumnType("jsonb")
                .HasColumnName("options");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .HasColumnName("unit");
            entity.Property(e => e.ValidationRules)
                .HasColumnType("jsonb")
                .HasColumnName("validation_rules");
        });

        modelBuilder.Entity<ClinFormField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clin_form_fields_pkey");

            entity.ToTable("clin_form_fields");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.FieldId).HasColumnName("field_id");
            entity.Property(e => e.IsRequired)
                .HasDefaultValue(false)
                .HasColumnName("is_required");
            entity.Property(e => e.LabelOverride)
                .HasMaxLength(100)
                .HasColumnName("label_override");
            entity.Property(e => e.TemplateId).HasColumnName("template_id");
            entity.Property(e => e.UiControl)
                .HasMaxLength(50)
                .HasColumnName("ui_control");

            entity.HasOne(d => d.Field).WithMany(p => p.ClinFormFields)
                .HasForeignKey(d => d.FieldId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("clin_form_fields_field_id_fkey");

            entity.HasOne(d => d.Template).WithMany(p => p.ClinFormFields)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("clin_form_fields_template_id_fkey");
        });

        modelBuilder.Entity<ClinFormTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clin_form_templates_pkey");

            entity.ToTable("clin_form_templates");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.Version)
                .HasDefaultValue(1)
                .HasColumnName("version");
        });

        modelBuilder.Entity<ClinMacro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clin_macros_pkey");

            entity.ToTable("clin_macros");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Expansion).HasColumnName("expansion");
            entity.Property(e => e.TriggerKey)
                .HasMaxLength(50)
                .HasColumnName("trigger_key");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.ClinMacros)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("clin_macros_user_id_fkey");
        });

        modelBuilder.Entity<ClinVital>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clin_vitals_pkey");

            entity.ToTable("clin_vitals");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.BpDiastolic).HasColumnName("bp_diastolic");
            entity.Property(e => e.BpSystolic).HasColumnName("bp_systolic");
            entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
            entity.Property(e => e.HeartRate).HasColumnName("heart_rate");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.RecordedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("recorded_at");
            entity.Property(e => e.RecordedBy).HasColumnName("recorded_by");
            entity.Property(e => e.Spo2).HasColumnName("spo2");
            entity.Property(e => e.Temperature)
                .HasPrecision(4, 1)
                .HasColumnName("temperature");

            entity.HasOne(d => d.Consultation).WithMany(p => p.ClinVitals)
                .HasForeignKey(d => d.ConsultationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("clin_vitals_consultation_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.ClinVitals)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("clin_vitals_patient_id_fkey");

            entity.HasOne(d => d.RecordedByNavigation).WithMany(p => p.ClinVitals)
                .HasForeignKey(d => d.RecordedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("clin_vitals_recorded_by_fkey");
        });

        modelBuilder.Entity<InvItem>(entity =>
        {
            entity.HasKey(e => e.Sku).HasName("inv_items_pkey");

            entity.ToTable("inv_items");

            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("sku");
            entity.Property(e => e.MinReorderLevel)
                .HasDefaultValue(0)
                .HasColumnName("min_reorder_level");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<InvTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inv_transactions_pkey");

            entity.ToTable("inv_transactions");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ChangeAmount).HasColumnName("change_amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Reason)
                .HasMaxLength(200)
                .HasColumnName("reason");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("sku");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.SkuNavigation).WithMany(p => p.InvTransactions)
                .HasForeignKey(d => d.Sku)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("inv_transactions_sku_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.InvTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("inv_transactions_user_id_fkey");
        });

        modelBuilder.Entity<LabResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lab_results_pkey");

            entity.ToTable("lab_results");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ReleasedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("released_at");
            entity.Property(e => e.ResultData)
                .HasColumnType("jsonb")
                .HasColumnName("result_data");
            entity.Property(e => e.ResultSummary).HasColumnName("result_summary");

            entity.HasOne(d => d.Order).WithMany(p => p.LabResults)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("lab_results_order_id_fkey");
        });

        modelBuilder.Entity<OrdOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ord_orders_pkey");

            entity.ToTable("ord_orders");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ConsultationId).HasColumnName("consultation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.OrderedBy).HasColumnName("ordered_by");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasColumnName("priority");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasOne(d => d.Consultation).WithMany(p => p.OrdOrders)
                .HasForeignKey(d => d.ConsultationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ord_orders_consultation_id_fkey");

            entity.HasOne(d => d.OrderedByNavigation).WithMany(p => p.OrdOrders)
                .HasForeignKey(d => d.OrderedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ord_orders_ordered_by_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.OrdOrders)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ord_orders_patient_id_fkey");
        });

        modelBuilder.Entity<PatPatient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pat_patients_pkey");

            entity.ToTable("pat_patients");

            entity.HasIndex(e => e.Mrn, "idx_pat_patients_mrn");

            entity.HasIndex(e => e.Mrn, "pat_patients_mrn_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ContactInfo)
                .HasColumnType("jsonb")
                .HasColumnName("contact_info");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .HasColumnName("gender");
            entity.Property(e => e.IsEmergencyReg).HasColumnName("is_emergency_reg");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Mrn)
                .HasMaxLength(20)
                .HasColumnName("mrn");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SchAppointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sch_appointments_pkey");

            entity.ToTable("sch_appointments");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.AppointmentDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("appointment_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.ReasonForVisit).HasColumnName("reason_for_visit");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Scheduled'::character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Doctor).WithMany(p => p.SchAppointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("sch_appointments_doctor_id_fkey");

            entity.HasOne(d => d.Patient).WithMany(p => p.SchAppointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sch_appointments_patient_id_fkey");
        });

        modelBuilder.Entity<SchEpisode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sch_episodes_pkey");

            entity.ToTable("sch_episodes");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");

            entity.HasOne(d => d.Patient).WithMany(p => p.SchEpisodes)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sch_episodes_patient_id_fkey");
        });

        modelBuilder.Entity<SysConfig>(entity =>
        {
            entity.HasKey(e => e.Key).HasName("sys_config_pkey");

            entity.ToTable("sys_config");

            entity.Property(e => e.Key)
                .HasMaxLength(100)
                .HasColumnName("key");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("user_roles_role_id_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("user_roles_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("user_roles_pkey");
                        j.ToTable("user_roles");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
