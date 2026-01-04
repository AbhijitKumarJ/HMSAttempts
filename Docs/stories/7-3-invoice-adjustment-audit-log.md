# Story 7.3: Invoice Adjustment & Audit Log

**Status:** ready-for-dev

## Story

As a Billing Clerk,
I want to manually adjust an invoice price,
So that I can apply discounts or correct errors.

## Acceptance Criteria

1.  **UI:** Invoice Detail View with editable line items.
2.  **Constraint:** Changing a price usually requires a "Reason Code" (e.g., "Charity Care", "Correction").
3.  **Audit:** The system MUST record the `OldPrice`, `NewPrice`, `Reason`, `UserId`, and `Timestamp` in `audit_logs`.
4.  **Feedback:** The Interface shows the updated total immediately.

## Technical Implementation

### Frontend
*   **Component:** `InvoiceDetailComponent`.
*   **UX:** Clicking a price opens an "Adjust Price" popover requiring the new price and a dropdown for Reason.

### Backend
*   **Endpoint:** `PATCH /api/billing/invoices/{id}/items/{itemId}`.
*   **Payload:** `{ "newPrice": 50.00, "reason": "indigent-care" }`.
*   **Logic:**
    *   Fetch Item.
    *   Create AuditLog entry.
    *   Update Item Price.
    *   Recalculate Invoice Total.
    *   Save Changes (Transaction).
