# Story 1.1: Project Initialization & Solution Setup

**Status:** ready-for-dev

## Story

As a Developer,
I want to initialize the .NET solution and Angular application,
So that the team has a pre-configured development environment.

## Acceptance Criteria

1.  **Solution Created:** A new `.sln` is created under `Backend/HMS_API`.
2.  **Projects Created:**
    *   `HMS.API` (ASP.NET Core Web API, .NET 9)
    *   `HMS.Business` (Class Library, .NET 9)
    *   `HMS.Entity` (Class Library, .NET 9)
    *   `HMS.Data` (Class Library, .NET 9)
3.  **Frontend Created:** A new Angular workspace (`Frontend/HMS_UI`) is initialized with Angular 21, SCSS, and Routing.
4.  **Database Ready:** A `docker-compose.yml` file is created for optional Docker usage. The application must support connecting to either a local PostgreSQL instance or a Dockerized one via configuration.
5.  **Unified Start:**
    *   Backend runs via `dotnet run` (port 5xxx).
    *   Frontend runs via `npm start` (port 4200).

## Technical Implementation

### Backend Setup
*   **Command:** `dotnet new sln -n HMS -o Backend/HMS_API`
*   **Command:** Scaffolding individual projects using `dotnet new webapi` and `dotnet new classlib`.
*   **References:** Link Business, Data, and Entity projects to the API project.

### Frontend Setup
*   **Command:** `npx @angular/cli@latest new HMS_UI --directory Frontend/HMS_UI --style=scss --routing --standalone`
*   **Cleanup:** Remove default Angular placeholder content.

### Infrastructure (Optional)
*   **File:** `root/docker-compose.yml` (provided for convenience).
*   **Service:** `db` (image: `postgres:18.1-alpine`).
*   **Config:** Expose port 5432, set standard `POSTGRES_USER`/`PASSWORD` in `.env` or direct config.

## Dependencies

*   .NET 9 SDK
*   Node.js (LTS)
*   PostgreSQL 18.1 (Local or Docker)
