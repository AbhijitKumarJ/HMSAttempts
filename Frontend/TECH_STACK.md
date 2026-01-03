Frontend Tech Stack (HMS)
=========================

Summary
-------

This project front end is a single-page application built with Angular and TypeScript. It uses the Angular build system and the Angular CLI for development and testing.

Key technologies
----------------

- Framework: Angular ^21.0.0
- Language: TypeScript (~5.9.x)
- Runtime: Modern browsers (target ES2022)
- Package manager: npm (packageManager: npm@11.6.2)
- Bundler / Build: Angular build system (`@angular/build`, `@angular/cli`)
- Testing: `vitest` (devDependency)

Repository evidence
-------------------

- `package.json` defines dependencies: `@angular/core@^21.0.0`, `typescript@~5.9.2`, `@angular/cli@^21.0.4`.
- `angular.json` configures an Angular application with source root `src`, builder `@angular/build:application`, and `tsConfig` set to `tsconfig.app.json`.
- `tsconfig.json` targets `ES2022` and enables strict TypeScript checks and Angular compiler strict options.

Dev & run commands
------------------

- Start development server: `npm start` (runs `ng serve`)
- Build production: `npm run build` (runs `ng build`)
- Watch dev build: `npm run watch` (runs `ng build --watch --configuration development`)
- Run unit tests: `npm test` (runs `ng test` — project also includes `vitest` for unit testing)

Notes & suggestions
--------------------

- The project uses Angular 21 and TypeScript 5.9 targeting ES2022 — ensure browsers targeted by your deployments support ES2022 features or enable appropriate polyfills if older browsers must be supported.
- `vitest` is present as a lightweight test runner. If the project relies on Angular CLI test runner, maintain consistency between frameworks or document how to run both.

Additional recommended packages
-------------------------------

Based on the architecture decisions (FHIR messaging, dynamic JSON form engine, optimistic UI, Signal/RxJS state patterns, OpenAPI contract-first approach), the frontend will benefit from these additional packages. Grouped install commands and short rationale are provided.

- UI & layout:
  - `@angular/material`, `@angular/cdk`, `@angular/animations` — Material components, accessibility helpers and animation support for a consistent, production-ready component set.

- State & reactivity:
  - `@ngrx/store`, `@ngrx/effects`, `@ngrx/entity`, `@ngrx/component-store` — Robust Redux-style state management for complex global state and side-effects. Use `@ngrx/component-store` for localized, service-level state.
  - `@ngrx/store-devtools` (dev) — time-travel and debugging during development.

- Dynamic forms & validation:
  - `@ngx-formly/core`, `@ngx-formly/material` — JSON-driven form rendering and mapping to Angular Material controls (ideal for JSONB clinical forms).
  - `ajv` — fast JSON Schema validation for dynamic form payloads and server-side contract validation.

- FHIR & Interoperability:
  - `fhirclient` (SMART on FHIR client) and `fhirpath` — helpers for working with FHIR resources, auth flows and evaluating FHIRPath expressions.
  - `fhir-kit-client` (optional) — utility client for calling FHIR servers if you prefer this API shape.

- API clients & codegen:
  - `openapi-typescript-codegen` or `@openapitools/openapi-generator-cli` (dev) — generate TypeScript/Angular clients from the backend OpenAPI spec to keep contracts in sync.

- Messaging & realtime:
  - `socket.io-client` — if using Socket.io for push updates. Otherwise the built-in `EventSource` (SSE) or `WebSocket` with `rxjs/webSocket` are sufficient.

- Testing & mocks:
  - `@testing-library/angular` — user-centric component tests.
  - `msw` (Mock Service Worker) — API mocking for unit/integration tests.
  - `playwright` or `cypress` — E2E testing (Playwright recommended for multi-browser CI).

- Dev tooling & quality:
  - `eslint`, `@angular-eslint/schematics`, `prettier`, `eslint-config-prettier`, `eslint-plugin-prettier` — consistent linting and formatting.

- Utilities:
  - `immer` — immutable update helpers for reducer code and optimistic UI updates.
  - `date-fns` (or `dayjs`) — lightweight, immutable date utilities (prefer over heavy `moment`).

Quick install examples
----------------------

Install runtime dependencies:

```
npm install @angular/material @angular/cdk @angular/animations @ngx-formly/core @ngx-formly/material ajv fhirclient fhirpath fhir-kit-client socket.io-client immer date-fns msw
```

Install state & dev tooling:

```
npm install @ngrx/store @ngrx/effects @ngrx/entity @ngrx/component-store
npm install --save-dev @ngrx/store-devtools openapi-typescript-codegen @openapitools/openapi-generator-cli @testing-library/angular playwright eslint @angular-eslint/schematics prettier eslint-config-prettier eslint-plugin-prettier
```

Generating API clients (example using OpenAPI Generator CLI):

```
npx @openapitools/openapi-generator-cli generate -i http://localhost:5000/swagger/v1/swagger.json -g typescript-angular -o src/app/generated/api
```

Or using `openapi-typescript-codegen` for a lightweight TypeScript client:

```
npx openapi-typescript-codegen --input http://localhost:5000/swagger/v1/swagger.json --output src/app/generated/api --client axios
```

Implementation notes
--------------------

- Choose one of the state approaches (NgRx or a Signals-based pattern). For rapid MVP work, `@ngrx/component-store` combined with Angular Signals (where appropriate) offers a smaller cognitive surface while still being testable and scalable.
- Use `@ngx-formly` + `ajv` to render and validate the dynamic clinical forms stored as JSONB in the backend.
- Use codegen to keep TypeScript DTOs in sync with the .NET backend OpenAPI; include generated clients in CI to detect contract drift early.
- Keep developer-only tools (code generators, devtools) in devDependencies to avoid shipping unnecessary packages.
