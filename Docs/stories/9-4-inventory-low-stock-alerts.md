# Story 9.4: Inventory Low Stock Alerts

**Status:** ready-for-dev

## Story

As an Inventory Manager,
I want to be notified when items run low,
So that I can reorder before we run out.

## Acceptance Criteria

1.  **Threshold:** Each `inv_item` has a `min_reorder_level` column.
2.  **Check:** When stock is dispensed (Story 6.2), check if `new_quantity < min_reorder_level`.
3.  **Alert:** If true, create a "System Alert" or "Notification" for the Inventory Manager role.
4.  **UI:** A "Low Stock" indicator on the Inventory Dashboard.

## Technical Implementation

### Database
*   Migration to add `min_reorder_level` to `inv_items`.

### Backend
*   Logic inside `InventoryService.DispenseAsync`.
*   (Optional) Daily background job `LowStockSweeper` to catch edge cases.
