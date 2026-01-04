# Story 9.7: Patient Visit Summary PDF

**Status:** ready-for-dev

## Story

As a Patient,
I want a PDF summary of my visit,
So that I have a record of my vitals and prescriptions.

## Acceptance Criteria

1.  **Action:** "Print Summary" button on the Patient Dashboard / Visit Detail.
2.  **Output:** A clean, branded PDF file downloaded to the browser.
3.  **Content:** Clinic Header, Patient Demographics, Vitals, Notes, Active Orders.

## Technical Implementation

### Backend
*   **Library:** `QuestPDF` (Open Source .NET library).
*   **Endpoint:** `GET /api/patients/{mrn}/visits/{id}/summary-pdf`.
*   **Logic:** Render PDF to `MemoryStream` and return `FileContentResult`.
