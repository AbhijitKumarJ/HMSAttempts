# Story 2.1: Patient Module & Database Schema

**Status:** ready-for-dev

## Story

As a Backend Developer,
I want to create the Patient module and database tables,
So that patient demographic data can be persisted securely.

## Acceptance Criteria

1.  **Project Created:** A new folder/namespace `HMS.Entity/Patient` is created.
2.  **Entity Definition:** `Patient.cs` entity is defined with EF Core attributes.
3.  **Table Schema:** `pat_patients` table is created via migration.
4.  **Columns:**
    *   `mrn` (VARCHAR, Unique Index, Primary Key or Alternate Key)
    *   `first_name`, `last_name` (VARCHAR)
    *   `dob` (DATE)
    *   `gender` (VARCHAR)
    *   `contact_info` (JSONB - optional for MVP)
5.  **Migration:** A reproducible EF Core migration script (`dotnet ef migrations add InitialPatient`) exists.

## Technical Implementation

### Entity Framework
*   **Context:** Update `HMSDbContext` to include `DbSet<Patient> Patients`.
*   **Configuration:** Use `IEntityTypeConfiguration<Patient>` to map to table `pat_patients`.
*   **Indexing:** Ensure `MRN` is indexed for fast lookups.

### Model
```csharp
public class Patient {
    public string Mrn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DoB { get; set; }
    public string Gender { get; set; }
}
```
