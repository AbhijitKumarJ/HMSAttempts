# Story 3.4: Vital Signs Alerting Logic

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Nurse,
I want immediate visual feedback on abnormal vitals based on the NEWS2 scoring system,
so that I can identify critical patients instantly during triage.

## Acceptance Criteria

1.  **Real-Time Scoring:** The frontend calculates a NEWS2 score in real-time as vitals (Resp Rate, SpO2, Temp, Systolic BP, Pulse Rate) are entered in the triage form.
2.  **Visual Highlighting:** Input fields turn Amber (Medium Risk) or Red (High Risk) immediately when an entered value falls outside the normal range defined by NEWS2.
    -   Score 1-2: Amber highlight.
    -   Score 3: Red highlight.
3.  **Aggregate Risk Indicator:** A "Risk Level" badge (Low, Medium, High) is displayed prominently on the form, updating based on the total NEWS2 score.
    -   Low (0-4): Green.
    -   Medium (5-6 or any single score of 3): Amber.
    -   High (7+): Red.
4.  **Backend Calculation:** The backend service re-calculates and persists the NEWS2 score within the `clin_assessments.data` JSONB payload upon "Finalize".
5.  **Alert Event:** If the final NEWS2 score is >= 5 (Medium/High Risk), a `Clinical.Alert` event is published to the message bus.
6.  **UX Standards:** Uses Angular Signals for efficient real-time calculation and Material design colors for semantic alerts.

## Tasks / Subtasks

- [ ] 1. Define NEWS2 Scoring Logic (Shared/Lib)
    - [ ] Create a utility/service (Frontend and Backend compatible) that implements the NEWS2 lookup table.
    - [ ] Account for "Scale 1" vs "Scale 2" for SpO2 (if patient has COPD - *MVP: Default to Scale 1*).
- [ ] 2. Frontend: Implement Real-Time Scorer
    - [ ] Update `triage-form` component to use Angular Signals.
    - [ ] Create a `computed()` signal that reacts to form control changes and returns the NEWS2 score and risk level.
- [ ] 3. Frontend: Dynamic Styling (JSON Forms Customization)
    - [ ] Create a custom renderer or use JSON Forms "Rules" to apply CSS classes (`news-warning`, `news-danger`) to fields based on their individual NEWS2 contribution.
- [ ] 4. Backend: Implement Alerting Service
    - [ ] Update `apps/api-server/src/clinical/service.py`.
    - [ ] Add `calculate_news2(data)` helper.
    - [ ] In `create_assessment`, if `status='FINAL'`, calculate score and publish `Clinical.Alert` event if threshold met.
- [ ] 5. UI: Risk Summary Badge
    - [ ] Add a summary section to the triage form showing the "Total NEWS2 Score" and "Action Recommended" (based on NEWS2 guidelines).
- [ ] 6. Unit Tests
    - [ ] Test 1: Verify scoring logic with edge cases (e.g., Resp Rate 25 -> Score 3).
    - [ ] Test 2: Verify aggregate score triggers correct risk level.
    - [ ] Test 3: Verify event publishing on high score.

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **Clinical Safety:** The scoring logic MUST be based on the official National Early Warning Score 2 (NEWS2) chart.
-   **Performance:** All calculations in the UI should happen in-memory using Signals to ensure < 100ms latency.
-   **JSONB Consistency:** Store the calculated `total_score` and `risk_level` inside the `data` field of the assessment so it's searchable via GIN index.
-   **Accessibility:** Ensure risk levels are indicated by both color AND text (e.g., "High Risk (7)") for colorblind users.

### Project Structure Notes

-   Shared Logic: Consider creating `libs/clinical-utils` if possible in Nx, otherwise duplicate logic in FE/BE for MVP with a "DO NOT DRIFT" comment.
-   Frontend: `apps/web-client/src/app/clinical/triage-form/`
-   Backend: `apps/api-server/src/clinical/service.py`

### References

-   [Source: _bmad-output/planning-artifacts/epics.md#story-34-vital-signs-alerting-logic]
-   [Source: Royal College of Physicians - NEWS2] (https://www.rcp.ac.uk/projects/outputs/national-early-warning-score-news-2)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
