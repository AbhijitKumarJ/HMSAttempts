# Story 4.3: Clinical Notes Editor

**Status:** ready-for-dev

## Story

As a Doctor,
I want to write free-text clinical notes,
So that I can document the consultation.

## Acceptance Criteria

1.  **Editor:** A dedicated text area for documentation.
2.  **Auto-Save:** Changes are saved locally or to draft API every 5-10 seconds of inactivity.
3.  **Finalize:** A "Sign & Lock" button persists the note to the DB and makes it immutable.
4.  **History:** Signed notes appear in the Patient Timeline.

## Technical Implementation

### Frontend
*   **Component:** `ClinicalNoteEditorComponent`.
*   **Forms:** Angular Reactive Forms (`FormControl`).
*   **Logic:**
    *   Use `valueChanges.pipe(debounceTime(5000))` to trigger auto-save drafts.
    *   Use `Validators.required` for final submission.

### Backend
*   **Endpoint:** `POST /api/clinical/notes` (Create Draft/Final).
*   **Endpoint:** `PUT /api/clinical/notes/{id}` (Update Draft).
*   **Entity:** `ClinicalNote` with property `IsFinalized`.
