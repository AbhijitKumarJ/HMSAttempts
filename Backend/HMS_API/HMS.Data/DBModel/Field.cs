using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Field
{
    public long Id { get; set; }

    public long? DynamicFormId { get; set; }

    public string? FieldName { get; set; }

    public string? FieldType { get; set; }

    public bool? IsRequired { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual DynamicForm? DynamicForm { get; set; }
}
