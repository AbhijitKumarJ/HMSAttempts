# Story 1.1: Project Initialization & Nx Workspace Setup

Status: ready-for-dev

<!-- Note: Validation is optional. Run validate-create-story for quality check before dev-story. -->

## Story

As a Developer,
I want to initialize the Nx monorepo with Angular and FastAPI,
so that the team has a unified, pre-configured development environment.

## Acceptance Criteria

1.  **Workspace Created:** A new Nx workspace is initialized containing `apps/web-client` (Angular) and `apps/api-server` (FastAPI).
2.  **Database Ready:** A `docker-compose.yml` file is created and successfully spins up PostgreSQL 18.1.
3.  **Unified Serve:** `nx serve` (or `nx run-many`) starts both frontend and backend servers successfully.
4.  **Language Support:** The workspace supports TypeScript/Angular for frontend and Python/FastAPI for backend via Poetry.
5.  **Environment:** Node.js, Python 3.11, and Docker are verified as prerequisites.

## Tasks / Subtasks

- [ ] 1. Verify Development Environment Prerequisites
    - [ ] Check Node.js version (LTS recommended)
    - [ ] Check Python version (3.11+)
    - [ ] Check Docker and Docker Compose availability
    - [ ] Install/Verify `poetry` is installed globally (`pip install poetry`)
- [ ] 2. Initialize Nx Workspace with Angular
    - [ ] Run architecture-specified init command: `npx create-nx-workspace@latest bmad-test --preset=angular --appName=web-client --style=scss --standaloneApi=true --routing=true`
    - [ ] Verify `apps/web-client` exists and is an Angular app
- [ ] 3. Add Python Support to Workspace
    - [ ] Install plugin: `npm install -D @nxlv/python`
    - [ ] Initialize plugin: `npx nx generate @nxlv/python:init`
- [ ] 4. Generate FastAPI Backend Application
    - [ ] Generate app: `npx nx generate @nxlv/python:poetry-project api-server --projectType=application --description="FastAPI Backend for BMADTest"`
    - [ ] Verify `apps/api-server` exists and contains `pyproject.toml`
- [ ] 5. Configure Docker Infrastructure
    - [ ] Create `docker-compose.yml` in project root
    - [ ] Define `db` service using `postgres:18.1` image
    - [ ] Configure environment variables for DB (POSTGRES_USER, POSTGRES_PASSWORD, POSTGRES_DB)
    - [ ] Map port 5432:5432
- [ ] 6. Verify End-to-End Startup
    - [ ] Start database: `docker-compose up -d db`
    - [ ] Serve apps: `npx nx run-many --target=serve --all` (or equivalent)
    - [ ] Verify Frontend accessible (usually http://localhost:4200)
    - [ ] Verify Backend accessible (usually http://localhost:8000)

## Dev Notes

- **Critical:** Follow the specific `npx` commands from the Architecture document to ensure correct plugin versions and presets.
- **Dependency Management:**
    - Frontend: `package.json` (npm/pnpm/yarn)
    - Backend: `apps/api-server/pyproject.toml` (Poetry)
- **Monorepo Structure:**
    - Ensure clear separation between `apps/web-client` and `apps/api-server`.
    - Do not mix language dependencies.
- **Troubleshooting:**
    - If `@nxlv/python` generation fails, check their documentation for latest flags.
    - Ensure Python 3.11 is on the system PATH.
    - If port conflicts occur, adjust in `project.json` (Angular) or FastAPI config, but default to 4200/8000.

### Project Structure Requirements

- Root: `nx.json`, `package.json`, `docker-compose.yml`
- Apps: `apps/web-client`, `apps/api-server`
- Backend Config: `apps/api-server/pyproject.toml`
- Frontend Config: `apps/web-client/project.json`

### Architecture Compliance

- **Frontend:** Angular 17+ (ensure init command pulls latest)
- **Backend:** Python 3.11+ / FastAPI
- **Database:** PostgreSQL 18.1
- **Build System:** Nx

### References

- [Source: _bmad-output/planning-artifacts/architecture.md#starter-template-evaluation] (Init commands)
- [Source: _bmad-output/planning-artifacts/epics.md#epic-1-foundations--secure-access] (Story requirements)

## Dev Agent Record

### Agent Model Used

Gemini 2.0 Flash

### Debug Log References

### Completion Notes List

### File List
