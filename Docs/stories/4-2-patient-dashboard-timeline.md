# Story 4.2: Patient Dashboard & Timeline

**Status:** ready-for-dev

## Story

As a Doctor,
I want a consolidated view of the patient's history and current status,
So that I can make informed decisions quickly.

## Acceptance Criteria

1.  **Layout:** 3-Column "Cockpit" Layout.
    *   **Left:** Navigation/Queue.
    *   **Center:** Pinned Vitals Ribbon + Chronological Timeline.
    *   **Right:** Active Action Area (Notes/Orders).
2.  **Pinned Ribbon:** Always visible header showing MRN, Name, Allergy Status, and Latest Vitals.
3.  **Timeline:** Scrollable list of previous visits, triage notes, and lab results.
4.  **Performance:** Dashboard loads critical data (Ribbon + Recent Timeline) in < 2 seconds.

## Technical Implementation

### Backend
*   **Endpoint:** `GET /api/patients/{mrn}/summary`
*   **Dto:** `PatientSummaryDto` containing Demographics, Allergies, ActiveProblems, and RecentTimelineEvents.
*   **Query:** Efficient EF Core projection (`.Select()`) to avoid over-fetching.

### Frontend
*   **Component:** `PatientDashboardComponent`.
*   **Timeline:** Use `@for` loop to render heterogeneous event types (Visit, Lab, Note).
*   **State:** Use `rxResource` or `Signal` based data fetching.
