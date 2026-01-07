namespace HMS.Entity.Clinical;

public class CreateUserEntity
{
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public long? RoleId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }    
}

public class UpdateUserEntity
{
    public long UserId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public long? RoleId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class GetUserEntity
{
    public long UserId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public long? RoleId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class FieldDefinitionDto
{
    public int Id { get; set; }
    public string FieldCode { get; set; } = null!;
    public string? Name { get; set; }
    public string? DataType { get; set; }
    public string? Unit { get; set; }
    public string? ValidationRules { get; set; }
    public string? Options { get; set; }
}

public class CreateFieldDefinitionDto
{
    public string FieldCode { get; set; } = null!;
    public string? Name { get; set; }
    public string? DataType { get; set; }
    public string? Unit { get; set; }
    public string? ValidationRules { get; set; }
    public string? Options { get; set; }
}

public class FormFieldDto
{
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public int FieldId { get; set; }
    public string? LabelOverride { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsRequired { get; set; }
    public string? UiControl { get; set; }
    public FieldDefinitionDto? Field { get; set; }
}

public class CreateFormFieldDto
{
    public int FieldId { get; set; }
    public string? LabelOverride { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsRequired { get; set; }
    public string? UiControl { get; set; }
}

public class FormTemplateDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool IsActive { get; set; }
    public int Version { get; set; }
    public List<FormFieldDto>? Fields { get; set; }
}

public class CreateFormTemplateDto
{
    public string? Title { get; set; }
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } = 1;
    public List<CreateFormFieldDto>? Fields { get; set; }
}

public class AssessmentValueDto
{
    public long Id { get; set; }
    public int FieldId { get; set; }
    public string? ValueRaw { get; set; }
    public string? ValueTyped { get; set; }
    public DateTime? CreatedAt { get; set; }
    public FieldDefinitionDto? Field { get; set; }
}

public class CreateAssessmentValueDto
{
    public int FieldId { get; set; }
    public string? ValueRaw { get; set; }
    public string? ValueTyped { get; set; }
}

public class AssessmentDto
{
    public long Id { get; set; }
    public int? PatientId { get; set; }
    public long? ConsultationId { get; set; }
    public int? TemplateId { get; set; }
    public DateTime? PerformedAt { get; set; }
    public int? PerformedBy { get; set; }
    public List<AssessmentValueDto>? Values { get; set; }
}

public class CreateAssessmentDto
{
    public int? PatientId { get; set; }
    public long? ConsultationId { get; set; }
    public int? TemplateId { get; set; }
    public List<CreateAssessmentValueDto>? Values { get; set; }
}

public class VitalsCaptureDto
{
    public int PatientId { get; set; }
    public long? ConsultationId { get; set; }
    public int? BpSystolic { get; set; }
    public int? BpDiastolic { get; set; }
    public int? HeartRate { get; set; }
    public decimal? Temperature { get; set; }
    public int? Spo2 { get; set; }
}

public class VitalsValidationResult
{
    public string Field { get; set; } = null!;
    public string AlertLevel { get; set; } = null!;
    public string Message { get; set; } = null!;
}

public class VitalsValidationResponse
{
    public bool IsValid { get; set; }
    public List<VitalsValidationResult>? Alerts { get; set; }
    public string? OverallPriority { get; set; }
}

public class VitalsAlertRule
{
    public string Field { get; set; } = null!;
    public string Condition { get; set; } = null!;
    public decimal Threshold { get; set; }
    public string AlertLevel { get; set; } = null!;
    public string Message { get; set; } = null!;
}

public class CreateEpisodeDto
{
    public int PatientId { get; set; }
    public string? Title { get; set; }
    public DateTime StartDate { get; set; }
}

public class EpisodeDto
{
    public long Id { get; set; }
    public int PatientId { get; set; }
    public string? Title { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Status { get; set; }
    public string? PatientName { get; set; }
}

public class StartConsultationDto
{
    public long AppointmentId { get; set; }
    public int DoctorId { get; set; }
    public int? EpisodeId { get; set; }
}

public class EndConsultationDto
{
    public string ClinicalSummary { get; set; } = null!;
}

public class LinkConsultationEpisodeDto
{
    public long EpisodeId { get; set; }
}

public class ConsultationDto
{
    public long Id { get; set; }
    public long? AppointmentId { get; set; }
    public long? EpisodeId { get; set; }
    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? ClinicalSummary { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorName { get; set; }
    public string? AppointmentStatus { get; set; }
    public string? EpisodeTitle { get; set; }
}