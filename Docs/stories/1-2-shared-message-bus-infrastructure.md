# Story 1.2: Shared Message Bus Infrastructure

**Status:** ready-for-dev

## Story

As a Backend Developer,
I want to implement the shared message bus library using PostgreSQL,
So that modules can communicate asynchronously without direct dependencies.

## Acceptance Criteria

1.  **Table Creation:** The system initializes the `app_events` table in PostgreSQL if it does not exist.
2.  **Publish Method:** A service method `Publish(string eventType, object payload)` exists to insert events with status `Pending`.
3.  **Consume Method:** A background service can fetch `Pending` events using `SKIP LOCKED` semantics to ensure safe concurrency.
4.  **Serialization:** Payloads are serialized to JSON (System.Text.Json) before storage.

## Technical Implementation

### Database Schema
*   **Table:** `app_events`
*   **Columns:**
    *   `id` (UUID, PK)
    *   `event_type` (VARCHAR(100), Indexed)
    *   `payload` (JSONB)
    *   `status` (VARCHAR(20) - 'Pending', 'Processing', 'Completed', 'Failed')
    *   `created_at` (TIMESTAMP DEFAULT NOW())
    *   `processed_at` (TIMESTAMP NULL)
    *   `failure_count` (INT DEFAULT 0)

### Generic Service (`HMS.Business/Services/EventBus.cs`)
*   **Interface:** `IEventBus`
*   **Implementation:**
    *   **Publish:** `INSERT INTO app_events (id, event_type, payload, status) VALUES (@id, @type, @json, 'Pending');`
    *   **Fetch:** Use the following `SKIP LOCKED` query pattern to ensure concurrency safety:
        ```sql
        UPDATE app_events
        SET status = 'Processing', processed_at = NOW()
        WHERE id = (
            SELECT id
            FROM app_events
            WHERE status = 'Pending' AND event_type = @type
            ORDER BY created_at ASC
            LIMIT 1
            FOR UPDATE SKIP LOCKED
        )
        RETURNING id, payload;
        ```
*   **Locking:** The `FOR UPDATE SKIP LOCKED` clause is critical. It allows multiple worker instances to poll the table simultaneously without blocking each other or processing the same row.

### Testing
*   Create a unit test where two concurrent tasks try to consume from the queue; only one should get the item.
