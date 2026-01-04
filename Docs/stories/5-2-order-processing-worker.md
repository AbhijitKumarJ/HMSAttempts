# Story 5.2: Order Processing Worker

**Status:** ready-for-dev

## Story

As a System,
I want to process "Order.Created" events,
So that the Lab module is notified of new work.

## Acceptance Criteria

1.  **Trigger:** Listens for `Order.Created` events in `app_events`.
2.  **Action:** Creates a record in the `lab_orders` table (managed by the Lab/Ancillary module).
3.  **Idempotency:** Ensures the same order isn't created twice if the message is redelivered.
4.  **Reliability:** Updates `app_events` status to `completed` only after the `lab_orders` record is successfully committed.

## Technical Implementation

### Backend
*   **Worker:** `LabOrderWorker : BackgroundService`.
*   **Logic:**
    1.  Poll `app_events` for `event_type = 'Order.Created'` and `status = 'pending'`.
    2.  Deserialize payload to `OrderCreatedEvent`.
    3.  Map to `LabOrder` entity.
    4.  Save to `LabDbContext`.
    5.  Mark event as `completed`.
