# Story 1.2: Shared Message Bus Infrastructure

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Backend Developer,
I want to implement the shared message bus library using PostgreSQL,
so that modules can communicate asynchronously without direct dependencies.

## Acceptance Criteria

1.  **Library Created:** A shared Python library `libs/message-bus` is created and installable/importable by `apps/api-server`.
2.  **Schema Defined:** The `app_events` table is defined using SQLAlchemy 2.0 (Async) with columns for `id`, `event_type`, `payload` (JSONB), `status` (pending/processing/completed/failed), `created_at`, and `processed_at`.
3.  **Publisher Implemented:** A `publish(event: str, payload: dict)` function exists that inserts events with `status='pending'`.
4.  **Consumer Implemented:** A `consume(event_type: str)` function exists that utilizes `FOR UPDATE SKIP LOCKED` to atomically fetch and lock pending events.
5.  **Payload Validation:** Event payloads are validated using Pydantic v2 models with `camelCase` alias configuration.
6.  **Async Support:** All database operations use `sqlalchemy.ext.asyncio`.
7.  **FIFO Processing:** Tests verify that events are processed in the order created (approximate, given concurrency).

## Tasks / Subtasks

- [ ] 1. Initialize `libs/message-bus` Library
    - [ ] Create directory structure `libs/message-bus`
    - [ ] Create `pyproject.toml` for the library (or configure within root if using a monorepo-style path package)
    - [ ] Register library in `apps/api-server/pyproject.toml` dependencies
- [ ] 2. Define Database Models
    - [ ] Create `libs/message-bus/src/models.py`
    - [ ] Define `AppEvent` SQLAlchemy model with `JSONB` payload column
    - [ ] Ensure proper indices on `status` and `event_type`
- [ ] 3. Implement Publisher Service
    - [ ] Create `libs/message-bus/src/publisher.py`
    - [ ] Implement `publish_event` async function accepting `AsyncSession`
    - [ ] Ensure strict Pydantic v2 model validation for payloads before insertion
- [ ] 4. Implement Consumer Service with SKIP LOCKED
    - [ ] Create `libs/message-bus/src/consumer.py`
    - [ ] Implement `fetch_next_event` using `select(AppEvent).where(...).with_for_update(skip_locked=True).limit(1)`
    - [ ] Implement context manager or utility to handle status transitions (pending -> processing -> completed)
- [ ] 5. Implement Base Pydantic Configuration
    - [ ] Create `libs/message-bus/src/schemas.py`
    - [ ] Define `BaseEventSchema` with `model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)`
- [ ] 6. Write Unit Tests
    - [ ] Configure `pytest` and `pytest-asyncio`
    - [ ] Test 1: Publish an event and verify it exists in DB
    - [ ] Test 2: Consume an event and verify status change to 'processing'
    - [ ] Test 3: Spawn two concurrent consumers and verify they don't process the same event (SKIP LOCKED verification)

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **SQLAlchemy 2.0 Syntax:** You MUST use the 2.0 style `select(...)`.
    ```python
    # CORRECT
    stmt = select(AppEvent).where(AppEvent.status == 'pending').with_for_update(skip_locked=True).limit(1)
    result = await session.execute(stmt)
    event = result.scalar_one_or_none()
    ```
-   **Pydantic V2 Config:** Ensure frontend compatibility by forcing camelCase.
    ```python
    from pydantic import BaseModel, ConfigDict
    from pydantic.alias_generators import to_camel

    class BaseSchema(BaseModel):
        model_config = ConfigDict(alias_generator=to_camel, populate_by_name=True)
    ```
-   **Library Isolation:** This code lives in `libs/message-bus`, NOT `apps/api-server`. The API server should import it.
-   **No External Brokers:** Do NOT install RabbitMQ or Redis. Use the Postgres table `app_events` as the queue.

### Project Structure Notes

-   **New Directory:** `libs/message-bus/` (Needs to be created).
-   **Integration:** `apps/api-server` will need to add this lib to its path or dependency list (e.g., via `tool.poetry.dependencies`).
-   **Database:** Use the shared `database.py` (if it exists) or create a simplified `engine` factory in the lib for testing.

### References

-   [Source: _bmad-output/planning-artifacts/architecture.md#core-architectural-decisions] (Queue Decision)
-   [Source: _bmad-output/planning-artifacts/epics.md#story-12-shared-message-bus-infrastructure] (Story Requirements)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
