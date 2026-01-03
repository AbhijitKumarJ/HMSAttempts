---
project_name: 'BMADTest'
user_name: 'Abhijit'
date: '2026-01-02'
sections_completed: ['technology_stack', 'language_rules', 'framework_rules', 'testing_rules', 'quality_rules', 'workflow_rules', 'anti_patterns']
status: 'complete'
rule_count: 18
optimized_for_llm: true
---

# Project Context for AI Agents

_This file contains critical rules and patterns that AI agents must follow when implementing code in this project. Focus on unobvious details that agents might otherwise miss._

---

## Technology Stack & Versions

- **Build System:** Nx Monorepo (Angular + FastAPI)
- **Frontend:** Angular 17+ (Signals, Standalone Components, RxJS)
- **Backend:** Python 3.11+ FastAPI (Async, Pydantic v2)
- **Database:** PostgreSQL 18.1 (JSONB, SKIP LOCKED)
- **Styling:** Tailwind CSS, SCSS, Angular Material
- **Communication:** Asynchronous HL7/FHIR messaging via Postgres Queue
- **Auth:** Standalone IdP module issuing context-aware JWTs

## Critical Implementation Rules

### Language-Specific Rules

- **Python:** MUST use `async def` for all I/O bound functions. Enforce `snake_case`.
- **TypeScript:** Enforce strict null checks. `camelCase` for variables/JSON keys.
- **JSON:** API payloads are ALWAYS `camelCase`. Backend must map `snake_case` models to `camelCase` JSON.

### Framework-Specific Rules

- **FastAPI:** Use `APIRouter` for modularity. Use Pydantic v2 `model_config` for schema definition.
- **Angular:** Components MUST be `standalone: true`. Use `OnPush` strategy. Prefer `signal()` over `BehaviorSubject`.

### Testing Rules

- **Backend:** `pytest` with `pytest-asyncio` for async routes.
- **Frontend:** `jest` specs co-located with components.

### Code Quality & Style Rules

- **Naming:** Files are `kebab-case` (e.g., `patient-profile.component.ts`).
- **Imports:** No circular dependencies between Nx libraries.

### Critical Don't-Miss Rules

- **Architecture:** "Internal-as-External" - Cross-module communication via `app_events` queue ONLY.
- **DB Access:** No direct DB access from frontend.
- **Optimistic UI:** UI updates immediately; background sync handles persistence.

---

## Usage Guidelines

**For AI Agents:**

- Read this file before implementing any code
- Follow ALL rules exactly as documented
- When in doubt, prefer the more restrictive option
- Update this file if new patterns emerge

**For Humans:**

- Keep this file lean and focused on agent needs
- Update when technology stack changes
- Review quarterly for outdated rules
- Remove rules that become obvious over time

Last Updated: 2026-01-02
