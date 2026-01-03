using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class DynamicForm
{
    public long DynamicFormId { get; set; }

    public string FormName { get; set; } = null!;

    public string? Fields { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();

    public virtual ICollection<Field> FieldsNavigation { get; set; } = new List<Field>();

    public virtual ICollection<FormField> FormFields { get; set; } = new List<FormField>();
}
