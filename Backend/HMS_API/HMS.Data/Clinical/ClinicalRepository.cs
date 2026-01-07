using HMS.Data.DBModel;
using HMS.Entity.Clinical;
using Microsoft.EntityFrameworkCore;

namespace HMS.Data.Clinical;

public interface IClinicalRepository
{
    object GetUserById(int id);
    
    FormTemplateDto CreateFormTemplate(CreateFormTemplateDto dto);
    FormTemplateDto? GetFormTemplate(int id);
    List<FormTemplateDto> GetFormTemplates();
    void DeleteFormTemplate(int id);
    
    ClinVital CreateVital(VitalsCaptureDto dto, int recordedBy);
    ClinVital? GetVital(long id);
    List<ClinVital> GetPatientVitals(int patientId);
}

public class ClinicalRepository : IClinicalRepository
{
    private readonly HMSContext _context;

    public ClinicalRepository(HMSContext context)
    {
        _context = context;
    }

    public object GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        return user ?? new object();
    }

    public FormTemplateDto CreateFormTemplate(CreateFormTemplateDto dto)
    {
        var template = new ClinFormTemplate
        {
            Title = dto.Title,
            IsActive = dto.IsActive,
            Version = dto.Version
        };

        _context.ClinFormTemplates.Add(template);
        _context.SaveChanges();

        if (dto.Fields != null && dto.Fields.Any())
        {
            var displayOrder = 0;
            foreach (var fieldDto in dto.Fields.OrderBy(f => f.DisplayOrder))
            {
                var formField = new ClinFormField
                {
                    TemplateId = template.Id,
                    FieldId = fieldDto.FieldId,
                    LabelOverride = fieldDto.LabelOverride,
                    DisplayOrder = fieldDto.DisplayOrder,
                    IsRequired = fieldDto.IsRequired,
                    UiControl = fieldDto.UiControl
                };
                _context.ClinFormFields.Add(formField);
                displayOrder++;
            }
            _context.SaveChanges();
        }

        return GetFormTemplate(template.Id)!;
    }

    public FormTemplateDto? GetFormTemplate(int id)
    {
        var template = _context.ClinFormTemplates
            .Include(t => t.ClinFormFields)
                .ThenInclude(f => f.Field)
            .FirstOrDefault(t => t.Id == id);

        if (template == null) return null;

        return new FormTemplateDto
        {
            Id = template.Id,
            Title = template.Title,
            IsActive = template.IsActive ?? true,
            Version = template.Version ?? 1,
            Fields = template.ClinFormFields
                .OrderBy(f => f.DisplayOrder)
                .Select(f => new FormFieldDto
                {
                    Id = f.Id,
                    TemplateId = f.TemplateId ?? 0,
                    FieldId = f.FieldId ?? 0,
                    LabelOverride = f.LabelOverride,
                    DisplayOrder = f.DisplayOrder ?? 0,
                    IsRequired = f.IsRequired ?? false,
                    UiControl = f.UiControl,
                    Field = f.Field != null ? new FieldDefinitionDto
                    {
                        Id = f.Field.Id,
                        FieldCode = f.Field.FieldCode,
                        Name = f.Field.Name,
                        DataType = f.Field.DataType,
                        Unit = f.Field.Unit,
                        ValidationRules = f.Field.ValidationRules,
                        Options = f.Field.Options
                    } : null
                }).ToList()
        };
    }

    public List<FormTemplateDto> GetFormTemplates()
    {
        return _context.ClinFormTemplates
            .Include(t => t.ClinFormFields)
                .ThenInclude(f => f.Field)
            .Select(t => new FormTemplateDto
            {
                Id = t.Id,
                Title = t.Title,
                IsActive = t.IsActive ?? true,
                Version = t.Version ?? 1,
                Fields = t.ClinFormFields
                    .OrderBy(f => f.DisplayOrder)
                    .Select(f => new FormFieldDto
                    {
                        Id = f.Id,
                        TemplateId = f.TemplateId ?? 0,
                        FieldId = f.FieldId ?? 0,
                        LabelOverride = f.LabelOverride,
                        DisplayOrder = f.DisplayOrder ?? 0,
                        IsRequired = f.IsRequired ?? false,
                        UiControl = f.UiControl,
                        Field = f.Field != null ? new FieldDefinitionDto
                        {
                            Id = f.Field.Id,
                            FieldCode = f.Field.FieldCode,
                            Name = f.Field.Name,
                            DataType = f.Field.DataType,
                            Unit = f.Field.Unit,
                            ValidationRules = f.Field.ValidationRules,
                            Options = f.Field.Options
                        } : null
                    }).ToList()
            }).ToList();
    }

    public void DeleteFormTemplate(int id)
    {
        var template = _context.ClinFormTemplates.FirstOrDefault(t => t.Id == id);
        if (template != null)
        {
            _context.ClinFormTemplates.Remove(template);
            _context.SaveChanges();
        }
    }

    public ClinVital CreateVital(VitalsCaptureDto dto, int recordedBy)
    {
        var vital = new ClinVital
        {
            PatientId = dto.PatientId,
            ConsultationId = dto.ConsultationId,
            BpSystolic = dto.BpSystolic,
            BpDiastolic = dto.BpDiastolic,
            HeartRate = dto.HeartRate,
            Temperature = dto.Temperature,
            Spo2 = dto.Spo2,
            RecordedBy = recordedBy,
            RecordedAt = DateTime.UtcNow
        };

        _context.ClinVitals.Add(vital);
        _context.SaveChanges();

        return vital;
    }

    public ClinVital? GetVital(long id)
    {
        return _context.ClinVitals
            .Include(v => v.Patient)
            .Include(v => v.Consultation)
            .Include(v => v.RecordedByNavigation)
            .FirstOrDefault(v => v.Id == id);
    }

    public List<ClinVital> GetPatientVitals(int patientId)
    {
        return _context.ClinVitals
            .Where(v => v.PatientId == patientId)
            .OrderByDescending(v => v.RecordedAt)
            .ToList();
    }
}