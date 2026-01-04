# Story 9.2: Patient Search & Command Palette

**Status:** ready-for-dev

## Story

As a Power User,
I want a global search bar accessible by keyboard shortcut (`Ctrl+K`),
So that I can find a patient or navigate to a module without using the mouse.

## Acceptance Criteria

1.  **Trigger:** `Ctrl+K` opens a modal overlay centered on screen.
2.  **Search:** Typing performs a fuzzy search on **Patients** (Name, MRN) and **Menu Items** (e.g., "Go to Pharmacy").
3.  **Results:** Shows top 5 matches with visual distinction (Icon for Patient vs Page).
4.  **Action:** Hitting Enter navigates to the selection.

## Technical Implementation

### Frontend
*   **Library:** Angular CDK `OverlayModule`.
*   **Component:** `CommandPaletteComponent`.
*   **API:** `GET /api/search?q=...` (Aggregated search endpoint).

### Backend
*   **Service:** `SearchService` that executes parallel queries (e.g., `EF.Functions.ILike` for Postgres) against Patient and Module tables.
