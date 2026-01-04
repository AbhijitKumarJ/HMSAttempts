# Story 5.1: Message Bus Publisher & Consumer Logic

**Status:** ready-for-dev

## Story

As a Developer,
I want to implement the core Event Publisher and Consumer services,
So that modules can exchange data reliably.

## Acceptance Criteria

1.  **Publisher Implementation:** A concrete implementation of `IEventBus` that writes to `app_events`.
2.  **Consumer Base Class:** A generic `EventConsumerBackgroundService<T>` that polls `app_events`.
3.  **Concurrency:** Implements `SKIP LOCKED` logic to ensure a message is processed by only one worker instance.
4.  **Status Transitions:** Updates message status from `pending` -> `processing` -> `completed` (or `failed`).

## Technical Implementation

### Backend
*   **Publisher:** `PostgresEventBus : IEventBus`. Method `PublishAsync(string eventName, object data)`.
*   **Consumer Host:** `BackgroundService` (Microsoft.Extensions.Hosting).
*   **Loop Logic:**
    *   `While(!stoppingToken.IsCancellationRequested)`
    *   **Poll:** Call `FetchNext`.
    *   **If Found:** Process immediately.
    *   **If Empty:** `await Task.Delay(1000)` (polling interval).
    *   **Error Handling:** If processing fails, increment `failure_count`. If `failure_count > 5`, mark status as `Failed` (Dead Letter).
*   **Transaction:** The `FetchNext` guarantees the lock. The status update to `Completed` should be done *after* the business logic succeeds.
