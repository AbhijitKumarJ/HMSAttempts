# Story 2.3: Patient Registration Frontend

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Receptionist,
I want a rapid registration form in the web client,
so that I can quickly register a patient without navigating complex menus.

## Acceptance Criteria

1.  **Trigger:** A prominent "Emergency Registration" button on the Dashboard.
2.  **Modal Dialog:** Clicking triggers an Angular Material Dialog (`MatDialog`).
3.  **Form Fields:** The form collects minimal data:
    -   First Name (Required, Text)
    -   Last Name (Required, Text)
    -   Gender (Required, Dropdown/Select: Male, Female, Other, Unknown)
4.  **Validation:** "Register" button is disabled until form is valid.
5.  **Submission:** On submit, sends `POST` request to `/api/patients/emergency`.
6.  **Feedback:**
    -   Shows loading spinner during submission.
    -   On success: Closes dialog, shows `MatSnackBar` toast "Patient Registered: [Name] ([MRN])", emits event to refresh patient list/queue.
    -   On failure: Shows error message within the dialog.
7.  **Responsive:** Dialog is responsive (fullscreen on mobile via CSS).
8.  **Keyboard Support:** Form supports `Enter` to submit.

## Tasks / Subtasks

- [ ] 1. Generate Component
    - [ ] Run `npx nx g @nx/angular:component patient/emergency-register-dialog --project=web-client`
    - [ ] Make it `standalone: true`.
    - [ ] Import `MatDialogModule`, `ReactiveFormsModule`, `MatFormFieldModule`, `MatInputModule`, `MatSelectModule`, `MatButtonModule`.
- [ ] 2. Implement Dialog UI
    - [ ] Template: `mat-dialog-title`, `mat-dialog-content` (Form), `mat-dialog-actions` (Cancel, Register).
    - [ ] Responsive CSS: Configure `panelClass: 'responsive-dialog'` in `MatDialogConfig` and add styles to `styles.scss` (fullscreen on mobile).
- [ ] 3. Implement Form Logic
    - [ ] Use `FormBuilder` to create `formGroup`.
    - [ ] Add validators: `Validators.required`.
    - [ ] Implement `onSubmit()`: Call `PatientService`.
- [ ] 4. Create Patient Service Method
    - [ ] Create/Update `apps/web-client/src/app/patient/patient.service.ts`.
    - [ ] Add `registerEmergency(data: PatientEmergencyCreate): Observable<Patient>`.
- [ ] 5. Integrate with Dashboard
    - [ ] Add "Emergency Registration" button to Dashboard component.
    - [ ] Implement `openRegistrationDialog()` using `MatDialog.open()`.
- [ ] 6. Unit Tests
    - [ ] Test form validation logic.
    - [ ] Test submission calls service.
    - [ ] Test error handling displays message.

## Dev Notes

### Technical Guardrails (CRITICAL)

-   **Standalone Components:** Ensure the dialog is a standalone component imported directly where needed (or lazy loaded).
-   **Reactive Forms:** Use `ReactiveFormsModule` explicitly.
-   **No Module Files:** Do NOT create `patient.module.ts`. Use `imports: [...]` in the component decorator.
-   **Responsive Dialogs:**
    -   Use `styles.scss` for the global `.responsive-dialog` class.
    -   Do NOT put dialog layout styles in component CSS (encapsulation prevents targeting the container).

### Project Structure Requirements

-   Directory: `apps/web-client/src/app/patient/`
-   Files: `emergency-register-dialog/emergency-register-dialog.component.ts|html|scss`, `patient.service.ts`

### References

-   [Source: _bmad-output/planning-artifacts/ux-design-specification.md#journey-1-the-multi-hat-rapid-triage-alex] (UX Flow)
-   [Source: Web Research] (Angular 17 Dialog Best Practices)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
