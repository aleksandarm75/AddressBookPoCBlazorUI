# Solution Structure, Local Setup, and Testing

## 1. Directory structure

```text
AddressBook-PoC/
├── AddressBook.sln
├── Directory.Build.props                 # net10.0, Nullable, implicit usings
├── README.md                             # Quick start guide
├── docs/
│   ├── ARCHITECTURE.md                   # Architecture diagrams and layer responsibilities
│   └── RUNBOOK.md                        # This document
├── scripts/
│   ├── start-api.ps1
│   └── start-web.ps1
└── src/
    ├── AddressBook.Contracts/             # Shared API/Web DTO contracts
    │   ├── AddressBook.Contracts.csproj
    │   ├── ContactDto.cs
    │   ├── CreateContactRequest.cs
    │   └── PagedResult.cs                 # Generic pagination DTO
    ├── AddressBook.Api/                   # ASP.NET Core Web API
    │   ├── AddressBook.Api.csproj
    │   ├── Program.cs                     # Dependency injection, CORS, seed data
    │   ├── Properties/launchSettings.json # http://localhost:5080
    │   ├── Controllers/ContactsController.cs
    │   ├── Facades/IContactsFacade.cs
    │   ├── Facades/ContactsFacade.cs
    │   ├── Providers/IContactsProvider.cs
    │   ├── Providers/EfContactsProvider.cs
    │   ├── Models/Contact.cs              # EF Core entity
    │   ├── Data/ContactsDbContext.cs
    │   └── Data/SampleData.cs             # 30 fictional US contacts
    └── AddressBook.Web/                   # Blazor WebAssembly
        ├── AddressBook.Web.csproj
        ├── Program.cs                     # DI and HttpClient BaseAddress
        ├── App.razor
        ├── Routes.razor
        ├── _Imports.razor
        ├── Properties/launchSettings.json # http://localhost:5081
        ├── Pages/Contacts.razor           # Contact list UI
        ├── Controllers/ContactsPageController.cs
        ├── Facades/IContactsUiFacade.cs
        ├── Facades/ContactsUiFacade.cs
        ├── Providers/IContactsApiProvider.cs
        ├── Providers/ContactsApiProvider.cs
        └── wwwroot/
            ├── index.html
            ├── appsettings.json           # API BaseAddress
            └── css/app.css
```

## 2. Prerequisites

- Windows 11 (or another supported operating system) with the **.NET 10 SDK**. `dotnet --version` should report `10.x` when this is the active SDK.
- PowerShell 7 or another terminal. VS Code and Visual Studio are optional.
- Access to **nuget.org** for the initial package restore.
- **Not required:** SQL Server, Docker, Node.js, or local EF migration tooling.

If `dotnet` is not installed, download the .NET 10 SDK from https://dotnet.microsoft.com/download/dotnet/10.0 and open a new terminal. On Windows, you can also install it using:

```powershell
winget install --id Microsoft.DotNet.SDK.10 --exact
```

## 3. Start the application (two terminals)

Extract the ZIP and open a terminal in `AddressBook-PoC`.

```powershell
# Optional: restore and build the complete solution
dotnet restore .\AddressBook.sln
dotnet build .\AddressBook.sln
```

**Terminal A — backend:**

```powershell
dotnet run --project .\src\AddressBook.Api --launch-profile http
```

The output should include `Now listening on: http://localhost:5080`.

**Terminal B — frontend:**

```powershell
dotnet run --project .\src\AddressBook.Web --launch-profile http
```

The output should include `Now listening on: http://localhost:5081`. Open **http://localhost:5081** in your browser.

Press `Ctrl+C` in both terminals to stop the application.

## 4. User walkthrough

1. Open the address book. The initial view displays **10 of the 30 fictional US contacts**.
2. Select page **2** or click **Next** to navigate to the second page. Enter `Seattle` and click **Search**. The frontend calls `?search=Seattle&page=1&pageSize=10`.
3. Click **Details** next to a contact to trigger `GET /api/contacts/{id}`.
4. Complete the **Add New Contact** form and click **Save Contact**. The frontend sends `POST /api/contacts` and reloads the list.
5. Restart the backend. Newly added contacts disappear and the original 30 seed contacts are restored.

## 5. API endpoint reference

| Method | URL | Response |
|---|---|---|
| GET | `/health` | `200 OK` with health status JSON |
| GET | `/api/contacts` | `200 OK` with paged results (default: first 10) |
| GET | `/api/contacts?search=Seattle&page=1&pageSize=10` | `200 OK` with filtered, paged results |
| GET | `/api/contacts?page=2&pageSize=10` | `200 OK` with the second page of 10 contacts |
| GET | `/api/contacts/{guid}` | `200 OK` with contact DTO, or `404 Not Found` |
| POST | `/api/contacts` | `201 Created` with Location and DTO, or `400 Bad Request` for invalid input |

**Example response for `GET /api/contacts?page=1&pageSize=10`:**

```json
{
  "items": [
    {
      "id": "(GUID)",
      "firstName": "Amelia",
      "lastName": "Anderson",
      "street": "805 Hillcrest Lane",
      "city": "Indianapolis, IN",
      "phone": "+1 (317) 555-0115",
      "email": "contact15@example.test"
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 30,
  "totalPages": 3
}
```

The `items` array above is intentionally abbreviated; a full first-page response contains 10 entries. All contacts are fictional.

Contact `id` values change when the API restarts because the seed process generates new GUIDs.

## 6. Test the API with PowerShell

```powershell
Invoke-RestMethod http://localhost:5080/health
$contacts = Invoke-RestMethod http://localhost:5080/api/contacts
$contacts.totalCount
$contacts.items.Count
Invoke-RestMethod 'http://localhost:5080/api/contacts?search=Seattle'
Invoke-RestMethod "http://localhost:5080/api/contacts/$($contacts.items[0].id)"
```

**Create a contact:**

```powershell
$body = @{
    firstName = 'Chris'; lastName = 'Walker'; street = '150 Main Street'
    city = 'Boston, MA'; phone = '+1 (617) 555-0199'; email = 'test@example.com'
} | ConvertTo-Json
Invoke-RestMethod -Method Post -Uri 'http://localhost:5080/api/contacts' -ContentType 'application/json' -Body $body
```

## 7. Troubleshooting

| Symptom | Check / solution |
|---|---|
| `dotnet` is not recognized | Install the .NET 10 SDK; open a new terminal; run `dotnet --info` |
| `NETSDK...` error during build | Ensure the installed SDK supports the `net10.0` target framework |
| NuGet restore fails | Check internet access, proxy/firewall configuration, and `nuget.org` package sources |
| Blazor loads but no contacts appear | Confirm the API works at `http://localhost:5080/health`; inspect Browser DevTools → Network |
| Browser reports a CORS error | Confirm the frontend runs at `http://localhost:5081`; update `WithOrigins` in the API `Program.cs` if needed |
| Port 5080 or 5081 is already in use | Stop the other process or change `launchSettings.json` and the matching API URL/CORS configuration |
| A new contact disappears after an API restart | Expected behavior: the InMemory database is not persistent |
| HTTPS mixed-content blocking | Use the default HTTP URLs for both projects or configure HTTPS consistently |

## 8. Where to make changes

- **Add a backend endpoint:** Extend `Api/Controllers`, followed by the corresponding facade and provider interfaces and implementations.
- **Integrate another backend service:** Add a new `I...Provider` interface and implementation, then orchestrate providers in the facade.
- **Replace InMemory with SQL:** Update the API `Program.cs`, EF configuration, and migrations. The UI contracts can remain unchanged.
- **Add a frontend use case:** Extend `Web/Controllers`, `Web/Facades`, and `Web/Providers`, then display the result in a Razor component.

## 9. Possible production enhancements

Consider xUnit tests with mocked providers, FluentValidation, OpenAPI/Swagger, advanced pagination, authentication, structured logging, retry/resilience policies, SQL Server or PostgreSQL, environment-based configuration, CI/CD, and deployment automation.
