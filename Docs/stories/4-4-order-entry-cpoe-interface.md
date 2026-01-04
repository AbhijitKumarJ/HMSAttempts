# Story 4.4: Order Entry (CPOE) Interface

**Status:** ready-for-dev

## Story

As a Doctor,
I want to search for and order Lab tests,
So that the patient can get diagnosed.

## Acceptance Criteria

1.  **Search:** Predictive search bar for Lab Tests and Medications.
2.  **Selection:** Selecting an item adds it to a "Pending Orders" list.
3.  **Submission:** Clicking "Sign Orders" submits the batch.
4.  **Feedback:** Optimistic UI immediately shows "Ordered" status while the backend processes the queue.
5.  **Event:** Submitting triggers `Order.Created` events on the Message Bus.

## Technical Implementation

### Frontend
*   **Component:** `OrderEntryComponent`.
*   **UI:** `MatAutocomplete` connected to a search API.
*   **State:** Local signal for `pendingOrders[]`.

### Backend
*   **Endpoint:** `GET /api/catalog/search?q=...` (Search Labs/Meds).
*   **Endpoint:** `POST /api/orders` (Submit Batch).
*   **Logic:**
    *   Save Order to DB.
    *   Publish `Order.Created` event for each item to `app_events`.
