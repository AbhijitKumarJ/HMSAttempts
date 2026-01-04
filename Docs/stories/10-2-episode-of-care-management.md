# Story 10.2: Episode of Care Management

**Status:** ready-for-dev

## Story

As a Doctor,
I want to group related visits into an "Episode of Care" (e.g., "Pregnancy 2024"),
So that I can track a condition over time.

## Acceptance Criteria

1.  **Action:** Create new Episode or Link Consultation to existing Episode.
2.  **View:** Patient Dashboard shows "Active Episodes".
3.  **Persistence:** Saves to `sch_episodes`.

## Technical Implementation

### Database
*   **Table:** `sch_episodes` (patient_id, title, start_date, status).

### Backend
*   **API:** `POST /episodes`, `PUT /episodes/{id}/close`.
*   **Link:** `clin_consultations` has `episode_id` FK.
