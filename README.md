# AddressBook PoC — Blazor + ASP.NET Core + EF Core InMemory

A compact demonstration of the **Controller → Facade → Provider** pattern on both sides of a web application: **Blazor WebAssembly (frontend)** and **ASP.NET Core Web API (backend)**. The API starts with **30 fictional US contacts** in its in-memory database. The application supports browsing, searching, viewing details, and adding contacts.

## Quick start (Windows 11 / PowerShell 7)

**Prerequisite:** .NET **10 SDK** (check with `dotnet --version`). Internet access is required for the initial NuGet restore and Blazor WebAssembly dependencies. No SQL Server, Docker, Node.js, or database migrations are required.

1. Extract the ZIP file into any directory.
2. Open **two PowerShell terminals** in the `AddressBook-PoC` project root.
3. Terminal 1 — start the backend:
   ```powershell
   dotnet run --project src/AddressBook.Api --launch-profile http
   ```
4. Terminal 2 — start the frontend:
   ```powershell
   dotnet run --project src/AddressBook.Web --launch-profile http
   ```
5. Open **http://localhost:5081** in your browser. The API is available at **http://localhost:5080**.

Alternatively, run `./scripts/start-api.ps1` and `./scripts/start-web.ps1` in separate terminals (provided your PowerShell execution policy allows scripts).

### Test the API independently of Blazor

```powershell
Invoke-RestMethod http://localhost:5080/health
Invoke-RestMethod http://localhost:5080/api/contacts
Invoke-RestMethod 'http://localhost:5080/api/contacts?search=Seattle&page=1&pageSize=10'
```

Retrieve an individual contact:

```powershell
$first = (Invoke-RestMethod http://localhost:5080/api/contacts).items[0]
Invoke-RestMethod "http://localhost:5080/api/contacts/$($first.id)"
```

Create a contact:

```powershell
$body = @{ firstName='Chris'; lastName='Walker'; street='150 Main Street'; city='Boston, MA'; phone='+1 (617) 555-0199'; email='test@example.com' } | ConvertTo-Json
Invoke-RestMethod -Method Post -Uri http://localhost:5080/api/contacts -ContentType 'application/json' -Body $body
```

## What the PoC demonstrates

- **Frontend:** `Contacts.razor` (View) → `ContactsPageController` → `ContactsUiFacade` → `ContactsApiProvider` (`HttpClient`).
- **Backend:** HTTP → `ContactsController` (Web API) → `ContactsFacade` → `EfContactsProvider` → `ContactsDbContext` (EF Core InMemory).
- **Shared contracts:** `AddressBook.Contracts` defines `ContactDto`, `CreateContactRequest`, and `PagedResult<T>`.
- **API endpoints:** `GET /api/contacts?page=1&pageSize=10`, `GET /api/contacts/{id}`, `POST /api/contacts`, and `GET /health`.
- **Server-side search, sorting, and pagination:** The EF Core provider applies filtering and stable ordering before `Skip`/`Take`. The UI displays **10 contacts per page** with **Previous / numbered pages / Next** navigation.
- The API returns `PagedResult<ContactDto>` containing `items`, `page`, `pageSize`, `totalCount`, and the calculated `totalPages`.
- **Temporary storage:** EF Core InMemory retains data only while the API is running. Restarting the API restores the original 30 seed contacts.

**Pagination example:** `http://localhost:5080/api/contacts?page=2&pageSize=10` returns the second page of up to 10 contacts.

## Documentation

- [Architecture, Mermaid diagrams, and layer responsibilities](docs/ARCHITECTURE.md)
- [Solution structure, setup, testing, and troubleshooting](docs/RUNBOOK.md)

## Important notes

This is a **proof of concept, not a production-ready template**. It does not include authentication, correlation-ID logging, persistent storage, automated tests, or a global exception-handling middleware. A production application would require more comprehensive validation and error handling.

The **frontend controller** is a manually implemented presentation/application controller, **not** an HTTP MVC controller. The HTTP controller exists only in the backend API.

**Configuration:** Local ports and CORS are set up for the default HTTP URLs. If you change the API port, update `src/AddressBook.Web/wwwroot/appsettings.json`. If you change the frontend port, update `WithOrigins` in `src/AddressBook.Api/Program.cs`.
