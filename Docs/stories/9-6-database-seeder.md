# Story 9.6: Database Seeder

**Status:** ready-for-dev

## Story

As a Developer,
I want a script to populate the database with realistic test data,
So that I can demo the application without manual data entry.

## Acceptance Criteria

1.  **Command:** `dotnet run --seed` (or similar argument).
2.  **Data:** Generates:
    *   10 Patients (various ages/genders).
    *   1 Clinical User, 1 Admin.
    *   10 Common Inventory Items (Tylenol, Amoxicillin).
    *   5 Clinical Form Templates (Vitals, Admission).
3.  **Idempotency:** Running it twice doesn't crash or duplicate unique constraints (uses `EnsureCreated` / Checks existence).

## Technical Implementation

### Backend
*   **Class:** `DataSeeder` service.
*   **Library:** `Bogus` (popular .NET faker library) to generate realistic names/addresses.
