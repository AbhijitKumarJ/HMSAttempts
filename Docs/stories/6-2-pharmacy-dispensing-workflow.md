# Story 6.2: Pharmacy Dispensing Workflow

**Status:** ready-for-dev

## Story

As a Pharmacist,
I want to dispense a medication for an order,
So that the patient receives their treatment.

## Acceptance Criteria

1.  **Trigger:** User clicks "Dispense" on a Pharmacy Order.
2.  **Validation:** Check if `quantity_on_hand >= ordered_quantity`.
3.  **Action:**
    *   Decrement stock in `inv_items`.
    *   Create `inv_transactions` record ("Dispensed Order #123").
    *   Publish `Medication.Dispensed` event to `app_events`.
4.  **Error:** Return error if insufficient stock.

## Technical Implementation

### Backend
*   **Controller:** `PharmacyController`.
*   **Service:** `InventoryService.DispenseAsync(sku, quantity)`.
*   **Transaction:** The decrement and event publish must be in an atomic Transaction.

### Frontend
*   **Button:** "Dispense" button on the Order Card.
*   **Feedback:** Toast notification "Dispensed successfully".
