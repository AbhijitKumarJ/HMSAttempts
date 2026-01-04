# Story 9.1: Clinical Macros (Dot Phrases)

**Status:** ready-for-dev

## Story

As a Doctor,
I want to use short commands (dot phrases) to insert common blocks of text,
So that I can document clinical notes faster and more consistently.

## Acceptance Criteria

1.  **Usage:** Typing `.` followed by a keyword (e.g., `.lungClear`) within the Clinical Note Editor instantly replaces the keyword with a preset text block.
2.  **Management:** Admins/Users can define personal or global macros (Key + Expansion Service).
3.  **UI:** A small autocomplete popup appears when the trigger character `.` is typed.

## Technical Implementation

### Database
*   **Table:** `clin_macros` (trigger_key, expansion_text, user_id, is_global).

### Frontend
*   **Component:** Enhance `ClinicalNoteEditorComponent`.
*   **Event:** Handle `textarea` input events to detect trigger char.
*   **State:** Load macros into a frontend Signal/Cache at startup for zero-latency expansion.
