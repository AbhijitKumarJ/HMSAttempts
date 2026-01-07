using HMS.Data.Clinical;
using HMS.Entity.Clinical;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace HMS.Business.Clinical;

public interface IClinicalService
{
    JObject GetUsers(int limit);
    JObject GetUser(int id);
    JObject CreateUser(CreateUserEntity entity);
    JObject UpdateUser(UpdateUserEntity entity);
    JObject DeleteUser(int id);
    
    FormTemplateDto CreateFormTemplate(CreateFormTemplateDto dto);
    FormTemplateDto? GetFormTemplate(int id);
    List<FormTemplateDto> GetFormTemplates();
    void DeleteFormTemplate(int id);
    
    VitalsCaptureDto CreateVital(VitalsCaptureDto dto, int recordedBy);
    VitalsCaptureDto? GetVital(long id);
    List<VitalsCaptureDto> GetPatientVitals(int patientId);
    VitalsValidationResponse ValidateVitals(VitalsCaptureDto dto);
    List<VitalsAlertRule> GetVitalsAlertRules();
}

public class ClinicalService : IClinicalService
{
    private readonly IClinicalRepository _clinicalRepository;

    public ClinicalService(IClinicalRepository clinicalRepository)
    {
        _clinicalRepository = clinicalRepository;
    }
    public JObject GetUsers(int limit)
    {
        return new JObject { ["Message"] = $"Retrieved {limit} users from Clinical service." };
    }

    public JObject GetUser(int id)
    {
        var userData = _clinicalRepository.GetUserById(id);
        return new JObject 
        { 
            ["Message"] = $"Retrieved user with ID {id} from Clinical service.",
            ["Data"] = JObject.FromObject(userData)
        };
    }

    public JObject CreateUser(CreateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Created user: {entity.Username} in Clinical service." };
    }

    public JObject UpdateUser(UpdateUserEntity entity)
    {
        return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in Clinical service." };
    }

    public JObject DeleteUser(int id)
    {
        return new JObject { ["Message"] = $"Deleted user with ID {id} in Clinical service." };
    }

    public FormTemplateDto CreateFormTemplate(CreateFormTemplateDto dto)
    {
        return _clinicalRepository.CreateFormTemplate(dto);
    }

    public FormTemplateDto? GetFormTemplate(int id)
    {
        return _clinicalRepository.GetFormTemplate(id);
    }

    public List<FormTemplateDto> GetFormTemplates()
    {
        return _clinicalRepository.GetFormTemplates();
    }

    public void DeleteFormTemplate(int id)
    {
        _clinicalRepository.DeleteFormTemplate(id);
    }

    public VitalsCaptureDto CreateVital(VitalsCaptureDto dto, int recordedBy)
    {
        var vital = _clinicalRepository.CreateVital(dto, recordedBy);
        return new VitalsCaptureDto
        {
            PatientId = vital.PatientId ?? 0,
            ConsultationId = vital.ConsultationId,
            BpSystolic = vital.BpSystolic,
            BpDiastolic = vital.BpDiastolic,
            HeartRate = vital.HeartRate,
            Temperature = vital.Temperature,
            Spo2 = vital.Spo2
        };
    }

    public VitalsCaptureDto? GetVital(long id)
    {
        var vital = _clinicalRepository.GetVital(id);
        if (vital == null) return null;
        
        return new VitalsCaptureDto
        {
            PatientId = vital.PatientId ?? 0,
            ConsultationId = vital.ConsultationId,
            BpSystolic = vital.BpSystolic,
            BpDiastolic = vital.BpDiastolic,
            HeartRate = vital.HeartRate,
            Temperature = vital.Temperature,
            Spo2 = vital.Spo2
        };
    }

    public List<VitalsCaptureDto> GetPatientVitals(int patientId)
    {
        var vitals = _clinicalRepository.GetPatientVitals(patientId);
        return vitals.Select(v => new VitalsCaptureDto
        {
            PatientId = v.PatientId ?? 0,
            ConsultationId = v.ConsultationId,
            BpSystolic = v.BpSystolic,
            BpDiastolic = v.BpDiastolic,
            HeartRate = v.HeartRate,
            Temperature = v.Temperature,
            Spo2 = v.Spo2
        }).ToList();
    }

    public VitalsValidationResponse ValidateVitals(VitalsCaptureDto dto)
    {
        var alerts = new List<VitalsValidationResult>();
        bool hasCritical = false;

        if (dto.BpSystolic.HasValue)
        {
            if (dto.BpSystolic > 180)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "BpSystolic",
                    AlertLevel = "Critical",
                    Message = "Systolic BP above 180 mmHg - Hypertensive Crisis"
                });
                hasCritical = true;
            }
            else if (dto.BpSystolic < 90)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "BpSystolic",
                    AlertLevel = "Critical",
                    Message = "Systolic BP below 90 mmHg - Hypotension"
                });
                hasCritical = true;
            }
            else if (dto.BpSystolic > 140)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "BpSystolic",
                    AlertLevel = "Warning",
                    Message = "Systolic BP elevated - Hypertension Stage 1"
                });
            }
        }

        if (dto.BpDiastolic.HasValue)
        {
            if (dto.BpDiastolic > 110)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "BpDiastolic",
                    AlertLevel = "Critical",
                    Message = "Diastolic BP above 110 mmHg - Hypertensive Crisis"
                });
                hasCritical = true;
            }
            else if (dto.BpDiastolic < 60)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "BpDiastolic",
                    AlertLevel = "Warning",
                    Message = "Diastolic BP below 60 mmHg"
                });
            }
        }

        if (dto.HeartRate.HasValue)
        {
            if (dto.HeartRate > 120)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "HeartRate",
                    AlertLevel = "Critical",
                    Message = "Heart rate above 120 bpm - Tachycardia"
                });
                hasCritical = true;
            }
            else if (dto.HeartRate < 50)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "HeartRate",
                    AlertLevel = "Critical",
                    Message = "Heart rate below 50 bpm - Bradycardia"
                });
                hasCritical = true;
            }
            else if (dto.HeartRate > 100)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "HeartRate",
                    AlertLevel = "Warning",
                    Message = "Heart rate elevated"
                });
            }
        }

        if (dto.Temperature.HasValue)
        {
            if (dto.Temperature > 39.0m)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "Temperature",
                    AlertLevel = "Critical",
                    Message = "Temperature above 39°C - High Fever"
                });
                hasCritical = true;
            }
            else if (dto.Temperature < 35.0m)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "Temperature",
                    AlertLevel = "Critical",
                    Message = "Temperature below 35°C - Hypothermia"
                });
                hasCritical = true;
            }
            else if (dto.Temperature > 38.0m)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "Temperature",
                    AlertLevel = "Warning",
                    Message = "Temperature elevated - Fever"
                });
            }
        }

        if (dto.Spo2.HasValue)
        {
            if (dto.Spo2 < 90)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "Spo2",
                    AlertLevel = "Critical",
                    Message = "SpO2 below 90% - Severe Hypoxemia"
                });
                hasCritical = true;
            }
            else if (dto.Spo2 < 94)
            {
                alerts.Add(new VitalsValidationResult
                {
                    Field = "Spo2",
                    AlertLevel = "Warning",
                    Message = "SpO2 below 94% - Mild Hypoxemia"
                });
            }
        }

        return new VitalsValidationResponse
        {
            IsValid = !hasCritical && alerts.Count == 0,
            Alerts = alerts,
            OverallPriority = hasCritical ? "High" : (alerts.Count > 0 ? "Medium" : "Normal")
        };
    }

    public List<VitalsAlertRule> GetVitalsAlertRules()
    {
        return new List<VitalsAlertRule>
        {
            new VitalsAlertRule
            {
                Field = "BpSystolic",
                Condition = ">",
                Threshold = 180,
                AlertLevel = "Critical",
                Message = "Systolic BP above 180 mmHg - Hypertensive Crisis"
            },
            new VitalsAlertRule
            {
                Field = "BpSystolic",
                Condition = "<",
                Threshold = 90,
                AlertLevel = "Critical",
                Message = "Systolic BP below 90 mmHg - Hypotension"
            },
            new VitalsAlertRule
            {
                Field = "BpSystolic",
                Condition = ">",
                Threshold = 140,
                AlertLevel = "Warning",
                Message = "Systolic BP elevated - Hypertension Stage 1"
            },
            new VitalsAlertRule
            {
                Field = "BpDiastolic",
                Condition = ">",
                Threshold = 110,
                AlertLevel = "Critical",
                Message = "Diastolic BP above 110 mmHg - Hypertensive Crisis"
            },
            new VitalsAlertRule
            {
                Field = "BpDiastolic",
                Condition = "<",
                Threshold = 60,
                AlertLevel = "Warning",
                Message = "Diastolic BP below 60 mmHg"
            },
            new VitalsAlertRule
            {
                Field = "HeartRate",
                Condition = ">",
                Threshold = 120,
                AlertLevel = "Critical",
                Message = "Heart rate above 120 bpm - Tachycardia"
            },
            new VitalsAlertRule
            {
                Field = "HeartRate",
                Condition = "<",
                Threshold = 50,
                AlertLevel = "Critical",
                Message = "Heart rate below 50 bpm - Bradycardia"
            },
            new VitalsAlertRule
            {
                Field = "HeartRate",
                Condition = ">",
                Threshold = 100,
                AlertLevel = "Warning",
                Message = "Heart rate elevated"
            },
            new VitalsAlertRule
            {
                Field = "Temperature",
                Condition = ">",
                Threshold = 39.0m,
                AlertLevel = "Critical",
                Message = "Temperature above 39°C - High Fever"
            },
            new VitalsAlertRule
            {
                Field = "Temperature",
                Condition = "<",
                Threshold = 35.0m,
                AlertLevel = "Critical",
                Message = "Temperature below 35°C - Hypothermia"
            },
            new VitalsAlertRule
            {
                Field = "Temperature",
                Condition = ">",
                Threshold = 38.0m,
                AlertLevel = "Warning",
                Message = "Temperature elevated - Fever"
            },
            new VitalsAlertRule
            {
                Field = "Spo2",
                Condition = "<",
                Threshold = 90,
                AlertLevel = "Critical",
                Message = "SpO2 below 90% - Severe Hypoxemia"
            },
            new VitalsAlertRule
            {
                Field = "Spo2",
                Condition = "<",
                Threshold = 94,
                AlertLevel = "Warning",
                Message = "SpO2 below 94% - Mild Hypoxemia"
            }
        };
    }
}

// public class ClinicalService : IClinicalService
// {
//     private readonly IClinicalRepository _clinicalRepository;

//     public ClinicalService(IClinicalRepository clinicalRepository)
//     {
//         _clinicalRepository = clinicalRepository;
//     }
//     public JObject GetUsers(int limit)
//     {
//         return new JObject { ["Message"] = $"Retrieved {limit} users from Clinical service." };
//     }

//     public JObject GetUser(int id)
//     {
//         var userData = _clinicalRepository.GetUserById(id);
//         return new JObject 
//         { 
//             ["Message"] = $"Retrieved user with ID {id} from Clinical service.",
//             ["Data"] = JObject.FromObject(userData)
//         };
//     }

//     public JObject CreateUser(CreateUserEntity entity)
//     {
//         return new JObject { ["Message"] = $"Created user: {entity.Username} in Clinical service." };
//     }

//     public JObject UpdateUser(UpdateUserEntity entity)
//     {
//         return new JObject { ["Message"] = $"Updated user with ID {entity.UserId} in Clinical service." };
//     }

//     public JObject DeleteUser(int id)
//     {
//         return new JObject { ["Message"] = $"Deleted user with ID {id} from Clinical service." };
//     }
// }