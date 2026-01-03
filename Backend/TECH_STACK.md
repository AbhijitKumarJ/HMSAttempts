Backend Tech Stack (HMS)
========================

Summary
-------

The backend is implemented in .NET 9 (ASP.NET Core) and is organized as a multi-project solution following a layered approach (API, Business, Entity, Data). It uses standard .NET Web APIs and JSON serialization packages.

Key technologies
----------------

- Runtime / Framework: .NET 9 / ASP.NET Core
- Language: C# (targeting `net9.0`)
- Web: `Microsoft.NET.Sdk.Web` (for the API project)
- JSON: `Microsoft.AspNetCore.Mvc.NewtonsoftJson` and `Newtonsoft.Json`
- Project layout: multiple projects referenced by the API (e.g., `HMS.Business`, `HMS.Entity`, `HMS.Data`)

Repository evidence
-------------------

- `HMS.API/HMS.API.csproj` targets `net9.0`, references `Microsoft.AspNetCore.Mvc.NewtonsoftJson` and `Microsoft.AspNetCore.OpenApi`, and references `HMS.Business` and `HMS.Entity` projects.
- `HMS.Business/HMS.Business.csproj` targets `net9.0` and references `HMS.Data` and `HMS.Entity` projects and contains `Newtonsoft.Json` as a package.

Dev & run commands
------------------

- Build solution: `dotnet build` (run from `Backend/HMS_API/HMS.sln` or from the project folders)
- Run API: `dotnet run --project Backend/HMS_API/HMS.API/HMS.API.csproj` or use `dotnet run` from within the API project folder.
- Tests: (no test projects detected in repo snapshot) — add test projects (xUnit/NUnit) if needed.

Database
--------

- The repository contains a `DB` folder with a README; the repo does not include explicit DB migration projects in the snapshot. The architecture expects a relational DB (PostgreSQL is a common choice for .NET apps) — confirm desired DB and add EF Core migrations or scripts as needed.

Notes & suggestions
--------------------

- Current packages and SDK target `.NET 9`. Ensure deployed environments have the .NET 9 runtime installed.
- Consider adding a `README` for backend run instructions and adding a `docker-compose` for local development including database services.
