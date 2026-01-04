# Story 6.3: Stock Management Interface

**Status:** ready-for-dev

## Story

As an Inventory Manager,
I want to manually update stock levels,
So that physical inventory matches the system.

## Acceptance Criteria

1.  **Grid:** A Data Grid showing all inventory items.
2.  **Edit:** Inline editing for "Quantity" and "UnitPrice".
3.  **Audit:** Changing a value prompts for a "Reason" (e.g., "Received Stock", "Damaged/Expired").
4.  **Save:** Commits the change and logs a transaction.

## Technical Implementation

### Frontend
*   **Component:** `InventoryGridComponent`.
*   **Library:** Angular Material Table or a light wrapper for inline editing.
*   **Dialog:** Open a "Reason" dialog on cell blur if value changed.

### Backend
*   **Endpoint:** `PATCH /api/inventory/items/{sku}`.
*   **Payload:** `{ "newQuantity": 100, "reason": "Restock" }`.
