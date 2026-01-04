# Story 6.1: Inventory Module & Stock Schema

**Status:** ready-for-dev

## Story

As a Backend Developer,
I want to create the Inventory module schema,
So that we can track medication stock levels.

## Acceptance Criteria

1.  **Project Created:** `HMS.Entity/Inventory` namespace.
2.  **Table:** `inv_items` created with columns:
    *   `sku` (Unique Index, String)
    *   `name` (String)
    *   `quantity` (Integer, Non-negative constraint)
    *   `unit_price` (Decimal)
3.  **Table:** `inv_transactions` created to audit changes (ItemId, ChangeAmount, Reason, UserId, Timestamp).
4.  **Relationships:** EF Core defined relations.

## Technical Implementation

### Entity Framework
*   **Entities:** `InventoryItem`, `InventoryTransaction`.
*   **Concurrency:** Use `Timestamp` / RowVersion to handle race conditions during updates.

### Migration
*   Run `dotnet ef migrations add InitialInventoryModule`.
