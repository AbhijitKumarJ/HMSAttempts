# Story 7.2: Automated Invoice Generation

**Status:** ready-for-dev

## Story

As a System,
I want to auto-create invoices from clinical events,
So that billing is accurate and immediate.

## Acceptance Criteria

1.  **Trigger:** Listens for `Medication.Dispensed` and `Lab.ResultAvailable` (or `Order.Completed`) events.
2.  **Logic:**
    *   **Idempotency Check:** Verify if an `InvoiceItem` for this `SourceEventId` already exists. If yes, skip.
    *   Find or Create an "Active" invoice for the patient.
    *   Look up price (from payload or Inventory Service).
    *   Add `InvoiceItem` to the invoice.
    *   Recalculate `Invoice.TotalAmount`.
3.  **Concurrency:** Handle potential race conditions if multiple items are added simultaneously (Db Lock or serialized worker).

## Technical Implementation

### Backend
*   **Worker:** `BillingEventWorker : BackgroundService`.
*   **Service:** `BillingService.AddLineItemAsync(mrn, itemDetails)`.
*   **Event Consumed:** `Medication.Dispensed`, `Lab.ResultAvailable`.

### Testing
*   Integration test: Publish `Medication.Dispensed` -> Verify `bil_invoice_items` count increases.
