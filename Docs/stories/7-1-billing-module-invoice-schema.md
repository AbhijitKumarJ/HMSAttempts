# Story 7.1: Billing Module & Invoice Schema

**Status:** ready-for-dev

## Story

As a Backend Developer,
I want to create the Billing module schema,
So that financial transactions can be stored.

## Acceptance Criteria

1.  **Project Created:** `HMS.Entity/Billing` namespace.
2.  **Tables:**
    *   `bil_invoices` (Id, PatientMrn, TotalAmount, Status).
    *   `bil_invoice_items` (Id, InvoiceId, Description, Quantity, UnitPrice, TotalPrice).
    *   `audit_logs` (Id, EntityId, EntityType, OldValue, NewValue, Reason, UserId, Timestamp).
3.  **Relationships:** EF Core defined relations (1:N for Invoice->Items).

## Technical Implementation

### Entity Framework
*   **Entities:** `Invoice`, `InvoiceItem`, `AuditLog`.
*   **Context:** `BillingDbContext` (or merged `HMSDbContext`).
*   **Audit:** Generic `AuditLog` entity to track changes across modules, but initially driven by Billing requirements.

### Migration
*   Run `dotnet ef migrations add InitialBillingModule`.
