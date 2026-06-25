# NZWalksAPI 🥾

> 📚 Built while following the Udemy course **[Build ASP.NET Core Web API - Scratch To Finish (.NET8 API)](https://addatech.udemy.com/course-dashboard-redirect/?course_id=4421508)**

A two-project .NET solution built with **ASP.NET Core**. One project is a fully featured **REST API** (NZWalks.API) and the other is a **server-side MVC web app** (NZWalks.UI) that consumes it. Together they form a complete full-stack system for managing hiking walks and regions across New Zealand.

---

## Solution Structure

```
NZWalksAPI.sln
├── NZWalks.API/          ← REST API (back end)
└── NZWalks.UI/           ← MVC Web App (front end, consumes the API)
```

Both projects run simultaneously — the UI talks to the API over HTTP using `HttpClient`.

---

## Project 1: NZWalks.API

The back end. A RESTful API that manages all data and enforces authentication.

### What It Manages
- **Regions** — geographic areas of New Zealand (e.g. Auckland, Wellington, Canterbury)
- **Walks** — hiking trails with a name, length, difficulty, and associated region
- **Difficulties** — difficulty levels (Easy, Medium, Hard) assigned to walks
- **Users** — registered accounts with role-based access (Reader / Writer)
- **Images** — image uploads stored locally

### Folder Structure

```
NZWalks.API/
├── Controllers/
│   ├── RegionsController.cs        # CRUD endpoints for regions
│   ├── WalksController.cs          # CRUD endpoints for walks
│   ├── DifficultiesController.cs   # CRUD endpoints for difficulty levels
│   ├── AuthController.cs           # Register & login → returns JWT token
│   └── ImagesController.cs         # Image upload handling
│
├── Models/
│   ├── Domain/                     # Core entities: Region, Walk, Difficulty, Image
│   └── DTO/                        # Request/response shapes (what the API exposes)
│
├── Repositories/
│   ├── IRegionRepository.cs        # Interface — decouples controller from DB
│   ├── SQLRegionRepository.cs      # EF Core implementation of region data access
│   ├── IWalkRepository.cs
│   ├── SQLWalkRepository.cs
│   └── (etc.)
│
├── Data/
│   ├── NZWalksDbContext.cs         # Main DB context (Walks, Regions, Difficulties)
│   └── NZWalksAuthDbContext.cs     # Identity DB context (Users, Roles)
│
├── Mappings/
│   └── AutoMapperProfiles.cs       # AutoMapper config: Domain ↔ DTO
│
├── Migrations/                     # EF Core generated migration files
│
├── Middlewares/
│   └── ExceptionHandlerMiddleware.cs  # Global error handling → clean JSON errors
│
├── appsettings.json                # DB connection strings, JWT config
└── Program.cs                      # DI registration, middleware pipeline
```

### API Endpoints

**Auth** — no token required
| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/auth/register` | Create a new user account |
| POST | `/api/auth/login` | Login and receive a JWT Bearer token |

**Regions** — `Reader` role to GET, `Writer` role to modify
| Method | Route |
|--------|-------|
| GET | `/api/regions` |
| GET | `/api/regions/{id}` |
| POST | `/api/regions` |
| PUT | `/api/regions/{id}` |
| DELETE | `/api/regions/{id}` |

**Walks** — supports query parameters
| Method | Route |
|--------|-------|
| GET | `/api/walks?filterOn=Name&filterQuery=track&sortBy=LengthInKm&isAscending=true&pageNumber=1&pageSize=10` |
| GET | `/api/walks/{id}` |
| POST | `/api/walks` |
| PUT | `/api/walks/{id}` |
| DELETE | `/api/walks/{id}` |

**Difficulties**
| Method | Route |
|--------|-------|
| GET | `/api/difficulties` |
| GET | `/api/difficulties/{id}` |
| POST | `/api/difficulties` |
| PUT | `/api/difficulties/{id}` |
| DELETE | `/api/difficulties/{id}` |

### Key Concepts in the API

- **Repository Pattern** — controllers never touch the DB directly; they call repository interfaces. This keeps them clean and testable.
- **Domain Models vs DTOs** — `Domain/` models map to DB tables; `DTO/` models are what clients send and receive. AutoMapper handles the translation.
- **Two DbContexts** — `NZWalksDbContext` for app data, `NZWalksAuthDbContext` for Identity (users/roles). Each has its own database.
- **JWT Authentication** — `POST /api/auth/login` returns a signed JWT. Every protected route validates it via `[Authorize]`.
- **Role-Based Authorization** — `[Authorize(Roles = "Reader")]` or `"Writer"` on controller actions.
- **Filtering, Sorting & Pagination** — implemented at the repository level using LINQ before hitting the DB.
- **Global Exception Middleware** — any unhandled exception is caught and returns a structured JSON error instead of crashing.
- **Async/Await throughout** — every DB call is non-blocking.
- **Serilog** — structured logging wired into the pipeline.

---

## Project 2: NZWalks.UI

The front end. An **ASP.NET Core MVC** web application that displays data from the API in a browser using Razor Views. It does not have its own database — it talks to NZWalks.API over HTTP.

### How It Works

```
Browser → NZWalks.UI (MVC Controller) → HttpClient → NZWalks.API → SQL Server
                                      ←  JSON response ←
         Razor View renders HTML ←
```

The UI uses `HttpClient` (registered via `IHttpClientFactory`) to make HTTP requests to the API, deserializes the JSON responses into view models, and passes them to Razor Views for rendering.

### Folder Structure

```
NZWalks.UI/
├── Controllers/
│   ├── RegionsController.cs    # Calls API /api/regions, renders region views
│   └── HomeController.cs       # Landing page
│
├── Views/
│   ├── Regions/
│   │   ├── Index.cshtml        # Table listing all regions
│   │   ├── Add.cshtml          # Form to create a region
│   │   ├── Edit.cshtml         # Form to edit a region
│   │   └── Delete.cshtml       # Confirm delete page
│   ├── Home/
│   │   └── Index.cshtml
│   └── Shared/
│       └── _Layout.cshtml      # Shared nav/layout wrapper
│
├── Models/
│   └── DTO/                    # View models that match API response shapes
│
├── appsettings.json            # API base URL config
└── Program.cs                  # HttpClient registration, middleware
```

### What the UI Does
- Lists all **Regions** in a table with action buttons (Edit, Delete)
- **Add Region** — submits a form which POSTs to the API
- **Edit Region** — pre-fills a form with existing data, PUTs to the API
- **Delete Region** — confirmation page, then DELETEs via the API

> The UI currently covers Regions. Walks and Difficulties are handled directly through the API (Swagger / Postman).

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local instance works fine)
- Visual Studio 2022 or VS Code

### 1. Clone the repo
```bash
git clone https://github.com/Josephlodico/NZWalksAPI.git
cd NZWalksAPI
```

### 2. Configure the API database connections
In `NZWalks.API/appsettings.json`:
```json
"ConnectionStrings": {
  "NZWalksConnectionString": "Server=YOUR_SERVER;Database=NZWalksDb;Trusted_Connection=True;TrustServerCertificate=True",
  "NZWalksAuthConnectionString": "Server=YOUR_SERVER;Database=NZWalksAuthDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 3. Configure JWT
In `NZWalks.API/appsettings.json`:
```json
"Jwt": {
  "Key": "your-long-secret-key-here-min-16-chars",
  "Issuer": "https://localhost:7047",
  "Audience": "https://localhost:7047"
}
```

### 4. Configure the UI to point at the API
In `NZWalks.UI/appsettings.json`:
```json
"NZWalksAPI": {
  "BaseUrl": "https://localhost:7047"
}
```
Make sure this matches the port your API actually runs on.

### 5. Apply EF Core migrations
```bash
cd NZWalks.API

dotnet ef database update --context NZWalksDbContext
dotnet ef database update --context NZWalksAuthDbContext
```

### 6. Run both projects
In Visual Studio: right-click the solution → **Set Startup Projects** → select both `NZWalks.API` and `NZWalks.UI` → run.

Or from two separate terminals:
```bash
# Terminal 1 — API
cd NZWalks.API && dotnet run

# Terminal 2 — UI
cd NZWalks.UI && dotnet run
```

| Project | Default URL |
|---------|-------------|
| NZWalks.API | `https://localhost:7047` |
| NZWalks.UI | `https://localhost:7001` (or similar) |
| Swagger UI | `https://localhost:7047/swagger` |

---

## Testing the API with Swagger

1. Open `https://localhost:7047/swagger`
2. `POST /api/auth/register` — create a user
3. `POST /api/auth/login` — copy the JWT token from the response
4. Click the **Authorize 🔒** button at the top of Swagger
5. Enter: `Bearer YOUR_TOKEN_HERE`
6. All protected endpoints are now unlocked

---

## Technologies Used

| Technology | Used In | Purpose |
|------------|---------|---------|
| ASP.NET Core 8 | Both | Web framework |
| ASP.NET Core MVC | UI | Controller + Razor View rendering |
| Entity Framework Core | API | ORM / database access |
| SQL Server | API | Database |
| ASP.NET Core Identity | API | User management |
| JWT Bearer Auth | API | Token-based authentication |
| AutoMapper | API | Domain ↔ DTO object mapping |
| HttpClient / IHttpClientFactory | UI | HTTP calls to the API |
| Swagger / Swashbuckle | API | API documentation & testing |
| Serilog | API | Structured logging |

---

## Course Reference

This project was built by following the Udemy course **[Build ASP.NET Core Web API - Scratch To Finish (.NET8 API)](https://addatech.udemy.com/course-dashboard-redirect/?course_id=4421508)**. The course covers building a production-style ASP.NET Core Web API from scratch, including EF Core, JWT auth, AutoMapper, and an MVC front end.

---

## What I Learned Building This

- How to build a proper layered REST API: Controllers → Repositories → EF Core → SQL Server
- How a separate MVC front end consumes a REST API using `HttpClient` instead of talking to a DB directly
- The difference between domain models and DTOs, and why keeping them separate matters
- How JWT authentication works end-to-end: issuing tokens on login, validating them on each request, enforcing roles per endpoint
- How to manage two separate EF Core DbContexts (app data vs identity data) in the same project
- How to implement filtering, sorting, and pagination in a repository using LINQ
- How global middleware cleanly handles exceptions across the whole API
- How AutoMapper eliminates repetitive property-copying code between model layers
