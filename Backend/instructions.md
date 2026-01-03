# Web API Project Standards

## 1. Project Structure
- Organize the project into clear folders: Controllers, Models, Data, etc.
- Use a consistent naming convention for files and folders.

## 2. Controller Standards
- Use `[ApiController]` attribute for all controllers to enable automatic model validation.
- Define routes clearly using `[Route]` attributes.
- Use only get and post ie HTTP verbs (`[HttpGet]`, `[HttpPost]`) for actions.
- Inject dependencies via constructor injection for better testability.

## 3. Database Configuration
- Use Entity Framework Core for database interactions.
- Configure the database connection string in the `appsettings.json` file instead of hardcoding it in the `Program.cs` file.
- Ensure the database is created if it does not exist during application startup.

## 4. JSON Serialization
- Use Newtonsoft.Json for JSON serialization and configure it to handle reference loops and null values appropriately. Use Microsoft.AspNetCore.Mvc.NewtonsoftJson package rather than just Newtonsoft.Json for this.

## 5. Middleware Configuration
- Configure middleware in the correct order: Static files, Routing, Authorization, etc.
- Use OpenAPI for API documentation in development mode.

## 6. Error Handling
- Implement global error handling to manage exceptions and return appropriate HTTP status codes.

## 7. Security
- Consider using HTTPS redirection in production environments.

## 8. Default Path
- By default have the default '/' path mapped to wwwroot folder's index.html 

## Additional Instructions
- Always place sensitive configurations, such as database connection strings, in the `appsettings.json` file to keep them secure and easily configurable.
- Use below nuget packages:

    API layer:
    dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 9.0.11


    Business Layer:
    dotnet add package Newtonsoft.Json --version 13.0.3

    Data Layer:
    dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.11
    dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.11
    dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.11

    Check if dotnet-ef tool available else install locally
    dotnet tool list --local
    dotnet tool install dotnet-ef --version 8.0.11 --create-manifest-if-needed
 
    For scaffolding: Ask user to use this command by changing username and password
    dotnet tool run dotnet-ef dbcontext scaffold "Host=localhost;Port=5432;Database=hms;Username=username;Password=password" Npgsql.EntityFrameworkCore.PostgreSQL -o DBModel -c DvdRentalContext -f --schema public


---