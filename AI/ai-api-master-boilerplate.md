# ENTERPRISE API MASTER BOILERPLATE
# For AI Code Generation — Follow ALL rules strictly

---

## STRICT RULES

- Always follow: Controller → Service → Repository → Translator
- Never put business logic in Controller
- Use async/await everywhere
- Always implement Interface for Service and Repository
- Repository returns DataTable ONLY — never Models or DTOs
- Translator converts DataTable → Model (separate static extension class)
- Use DTOs for all request/response — never expose Models directly
- Proper logging using ILogger<T> — inject via constructor
- Exception handling mandatory in every layer (try/catch + LogError)
- Use SqlParamHelper.Param() for all SQL parameters — see Helpers/SqlParamHelper.cs
- Use ApiResponse<T> wrapper for ALL controller responses — see Common/ApiResponse.cs
- Define ALL routes in ApiRoutes static class — see Common/ApiRoutes.cs
- Constructor injection only

---

## NAMING CONVENTION — MOST IMPORTANT

All layers must derive from the Controller name. NEVER mix names across layers.

| Layer                  | Convention                        | Example (Name = Salary)           |
|------------------------|-----------------------------------|-----------------------------------|
| Controller             | `{Name}Controller`                | `SalaryController`                |
| Service Interface      | `I{Name}Service`                  | `ISalaryService`                  |
| Service Implementation | `{Name}Service`                   | `SalaryService`                   |
| Repository Interface   | `I{Name}Repository`               | `ISalaryRepository`               |
| Repository Impl        | `{Name}Repository`                | `SalaryRepository`                |
| Translator             | `{Name}Translator`                | `SalaryTranslator`                |
| DTOs file              | `{Name}DTOs.cs`                   | `SalaryDTOs.cs`                   |
| ApiRoutes entry        | `ApiRoutes.{Name}`                | `ApiRoutes.Salary`                |
| DI Registration        | `I{Name}Service / {Name}Service`  | `ISalaryService / SalaryService`  |

---

## HOW TO USE THIS BOILERPLATE
<!-- FOR HUMAN REFERENCE ONLY — AI: ignore this section -->

```
Using the ai-api-master-boilerplate.md, generate a full API for the following:
Controller Name : {Name}Controller         → e.g. SalaryController
HTTP Verb       : GET / POST / PUT / DELETE
Feature         : short description        → e.g. Get salary of an employee
Request Fields  : field name (type)        → e.g. EmployeeId (Guid)     [omit if GET All]
Response Fields : field name (type)        → e.g. EmployeeId (Guid), BasicSalary (decimal)
Stored Procedure: sp_{Name}               → e.g. sp_GetSalaryOfEmployee
Base Route      : api/{name}              → e.g. api/salary
Method Route    : short route             → e.g. get
```

---

## COMPACT PATTERN — GET with parameters (reference for all layers)

> **Token key:** `{Name}` = PascalCase (e.g. `Case`), `{name}` = camelCase (e.g. `case`), `{Feature}` = method name (e.g. `GetAllCases`)

### File: Common/ApiRoutes.cs — add entry
```csharp
public static class {Name}
{
    public const string Base      = "api/{name}";
    public const string {Method}  = "{method}";
}
```

### File: Models/DTOs/{Name}DTOs.cs
```csharp
public class {Feature}Request  { public {type} {Field} { get; set; } }   // omit if GET All
public class {Name}Response    { public {type} {Field} { get; set; } }
```

### File: Controllers/{Name}Controller.cs
```csharp
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route(ApiRoutes.{Name}.Base)]
public class {Name}Controller : ControllerBase
{
    private readonly I{Name}Service            _{name}Service;
    private readonly ILogger<{Name}Controller> _logger;
    public {Name}Controller(I{Name}Service {name}Service, ILogger<{Name}Controller> logger)
    { _{name}Service = {name}Service; _logger = logger; }

    [Http{Verb}(ApiRoutes.{Name}.{Method})]
    public async Task<IActionResult> {Feature}([FromQuery] {Feature}Request request)  // [FromBody] for POST/PUT
    {
        try
        {
            var result = await _{name}Service.{Feature}Async(request);
            return Ok(ApiResponse<List<{Name}Response>>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {Feature}");
            return StatusCode(500, ApiResponse<object>.Fail("Internal Server Error"));
        }
    }
}
```

### File: Services/Interfaces/I{Name}Service.cs
```csharp
public interface I{Name}Service
{
    Task<List<{Name}Response>> {Feature}Async({Feature}Request request);  // omit parameter entirely if GET All (no Request Fields)
}
```

### File: Services/{Name}Service.cs
```csharp
public class {Name}Service : I{Name}Service
{
    private readonly I{Name}Repository      _{name}Repository;
    private readonly ILogger<{Name}Service> _logger;
    public {Name}Service(I{Name}Repository {name}Repository, ILogger<{Name}Service> logger)
    { _{name}Repository = {name}Repository; _logger = logger; }

    public async Task<List<{Name}Response>> {Feature}Async({Feature}Request request)
    {
        try
        {
            var table = await _{name}Repository.{Feature}Async(request);
            if (table == null || table.Rows.Count == 0) return new List<{Name}Response>();
            return table.To{Name}ResponseList();
        }
        catch (Exception ex) { _logger.LogError(ex, "Service Error in {Feature}Async"); throw; }
    }
}
```

### File: Repositories/Interfaces/I{Name}Repository.cs
```csharp
using System.Data;
public interface I{Name}Repository
{
    Task<DataTable> {Feature}Async({Feature}Request request);  // omit parameter entirely if GET All (no Request Fields)
}
```

### File: Repositories/{Name}Repository.cs
```csharp
using System.Data;
using Microsoft.Data.SqlClient;
public class {Name}Repository : I{Name}Repository
{
    private readonly IDbHelper               _dbHelper;
    private readonly ILogger<{Name}Repository> _logger;
    public {Name}Repository(IDbHelper dbHelper, ILogger<{Name}Repository> logger)
    { _dbHelper = dbHelper; _logger = logger; }

    public async Task<DataTable> {Feature}Async({Feature}Request request)
    {
        try
        {
            var parameters = new[]
            {
                SqlParamHelper.Param("@{Field}", request.{Field}, SqlDbType.{Type})
                // Use Array.Empty<SqlParameter>() if GET All (no params)
            };
            return await _dbHelper.ExecuteReaderAsync("sp_{StoredProc}", parameters, CommandType.StoredProcedure);
            // Use ExecuteNonQueryAsync for POST / PUT / DELETE
        }
        catch (Exception ex) { _logger.LogError(ex, "Repository Error in {Feature}Async"); throw; }
    }
}
```

### File: Translators/{Name}Translator.cs
```csharp
using System.Data;
public static class {Name}Translator
{
    public static List<{Name}Response> To{Name}ResponseList(this DataTable table)
    {
        if (table == null || table.Rows.Count == 0) return new List<{Name}Response>();
        return table.AsEnumerable().Select(row => new {Name}Response
        {
            {Field} = row.Field<{type}>("{ColumnName}")
        }).ToList();
    }
}
```

### File: Program.cs — add DI registrations
```csharp
builder.Services.AddScoped<I{Name}Service,    {Name}Service>();
builder.Services.AddScoped<I{Name}Repository, {Name}Repository>();
builder.Services.AddScoped<IDbHelper,         DbHelper>();   // shared — register once
```

---

## HTTP VERB QUICK REFERENCE

| Verb   | Attribute     | [From...]       | DbHelper method          | Return type       |
|--------|---------------|-----------------|--------------------------|-------------------|
| GET    | `[HttpGet]`   | `[FromQuery]`   | `ExecuteReaderAsync`     | `List<T>` or `T`  |
| POST   | `[HttpPost]`  | `[FromBody]`    | `ExecuteNonQueryAsync`   | `bool`            |
| PUT    | `[HttpPut]`   | `[FromBody]`    | `ExecuteNonQueryAsync`   | `bool`            |
| DELETE | `[HttpDelete]`| `[FromQuery]`   | `ExecuteNonQueryAsync`   | `bool`            |

---

## GENERATION ORDER — MANDATORY

When generating, always produce files in this order:
1. `ApiRoutes.cs` — add new `{Name}` entry
2. `Models/DTOs/{Name}DTOs.cs`
3. `Controllers/{Name}Controller.cs`
4. `Services/Interfaces/I{Name}Service.cs`
5. `Services/{Name}Service.cs`
6. `Repositories/Interfaces/I{Name}Repository.cs`
7. `Repositories/{Name}Repository.cs`
8. `Translators/{Name}Translator.cs`
9. `Program.cs` — add DI registrations

Verify before finishing:
- All layer names match the Controller name
- `ApiResponse<T>` used in all controller responses
- Repository always returns `DataTable`
- `using System.Data;` present in repository interface and implementation
- `try/catch` + `ILogger.LogError` in every layer
- Constructor injection in every layer

---

## CODE GENERATION RULE — MANDATORY

NEVER output generated code in the chat window.
ALWAYS write code directly into the solution files using file write tools.

Steps before writing any code:
1. Explore the solution (Glob *.cs, read an existing controller + Program.cs for folder layout and patterns)
2. Match exact folder paths used by existing layers
3. Write each file directly into the correct folder
4. Update `ApiRoutes.cs` and `Program.cs` in-place — do not recreate them
5. Show only a short summary table of files created/updated

---

## BUILD VERIFICATION RULE — MANDATORY

After ALL files are written, ALWAYS run a build automatically.

1. Locate the `.sln` or `.csproj` in the solution root
2. Run: `dotnet build`
3. Build PASSES → report "Build passed" in the summary
4. Build FAILS → read errors, fix affected files, rebuild until clean
5. Never leave the solution in a broken state

DO NOT ask the user to build manually. Building and fixing errors is part of code generation.
