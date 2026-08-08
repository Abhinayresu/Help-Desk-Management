# HelpDeskManagement - Support Ticket System

A clean, decoupled enterprise-grade Help Desk Ticket Management System built using **ASP.NET Core Web API**, **ASP.NET Core MVC Client**, and **xUnit automated unit testing**. The architecture enforces separation of concerns through the **Repository Pattern** and communicates asynchronously over RESTful HTTP interfaces.

---

## 👨‍💻 Student Information
* **Student Name:** RESU ABHINAY
* **Student ID:** 23BAI10545
* **GitHub Username:** Abhinay resu
* **GitHub Repository:** https://github.com/Abhinayresu/Help-Desk-Management.git

---

## 🛠️ Tech Stack & Key Frameworks
* **Framework:** .NET 10.0 (C#)
* **Web API Backend:** ASP.NET Core REST API
* **Client Frontend:** ASP.NET Core MVC (Razor Views, Bootstrap 5, Bootstrap Icons)
* **ORM:** Entity Framework Core (EF Core)
* **Database:** Microsoft SQL Server
* **Unit Testing:** xUnit & Moq (Mock Repository Layer)

---

## 🏗️ Solution Architecture & Structure

The solution contains three primary projects:

```text
HelpDeskManagement
│
├── HelpDesk.Api (ASP.NET Core REST Web API Backend)
│   ├── Controllers/       - TicketController exposes REST endpoints
│   ├── Data/              - HelpDeskDbContext configuration and initial seeding
│   ├── Models/            - Core Ticket entity
│   └── Repositories/      - ITicketRepository and TicketRepository implementation
│
├── HelpDesk.Mvc (ASP.NET Core MVC Web Application Client)
│   ├── Controllers/       - TicketController (interacts only with TicketService)
│   ├── Models/            - TicketViewModel and DashboardViewModel
│   ├── Services/          - TicketService using HttpClient to consume REST APIs
│   └── Views/             - Razor views for Dashboard, List, Details, Create, Edit, and Delete
│
└── HelpDesk.Tests (Automated Test Suite)
    └── TicketControllerTests.cs - Independent unit tests mocking repository calls
```

---

## 📡 Web API Endpoints (`HelpDesk.Api`)

All endpoints are hosted asynchronously under `http://localhost:5088/` / `https://localhost:7008/` by default:

| HTTP Method | API Endpoint Route | Description |
| :--- | :--- | :--- |
| `GET` | `/api/Ticket/All` | Retrieve all support tickets |
| `GET` | `/api/Ticket/{id}` | Retrieve a specific ticket by ID |
| `POST` | `/api/Ticket` | Raise a new ticket (Status is set to `Open` by default) |
| `PUT` | `/api/Ticket/{id}` | Update an existing ticket details |
| `DELETE` | `/api/Ticket/{id}` | Remove a ticket permanently from database |
| `GET` | `/api/Ticket/Status/{status}` | Retrieve tickets filtered by Status |

---

## 💻 MVC User Interface Features (`HelpDesk.Mvc`)

* **Dashboard View**: Real-time counter statistics for Total Tickets, Open Tickets, In Progress, and Closed Tickets.
* **All Tickets Table**: Interactive dashboard listing tickets with custom-designed visual badges for Priority and Status levels.
* **Filter Tickets**: Status-based search dropdown to filter open, in-progress, or closed tickets.
* **Raise New Ticket Form**: Allows submitting technical issues. Status is automatically assigned to `Open` and Priority is selectable via a dropdown.
* **Edit Ticket Form**: Supports updating the Title, Description, Priority level, and Status.
* **Delete Confirmation**: Secure delete validation screen prior to permanent ticket destruction.

---

## 💾 Database Setup & Migration Instructions

The project uses SQL Server database context via Entity Framework Core. To configure and create the database:

1. Update the database connection string in `HelpDesk.Api/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HelpDeskDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```
2. Navigate to the project root and add migrations:
   ```bash
   dotnet ef migrations add InitialCreate --project HelpDesk.Api
   ```
3. Update the database schema:
   ```bash
   dotnet ef database update --project HelpDesk.Api
   ```

---

## 🚀 How to Run the Applications

### 1. Build the Entire Solution
```bash
dotnet build HelpDeskManagement.sln
```

### 2. Run the REST Web API Backend
```bash
dotnet run --project HelpDesk.Api/HelpDesk.Api.csproj
```
The API Swagger documentation will be available locally.

### 3. Run the MVC Client Frontend
```bash
dotnet run --project HelpDesk.Mvc/HelpDesk.Mvc.csproj
```
Open a browser and navigate to the printed localhost address (e.g., `http://localhost:5242` or as configured).

### 4. Run Automated Unit Tests
```bash
dotnet test HelpDesk.Tests/HelpDesk.Tests.csproj
```
All xUnit tests will execute mocking the DB layers.
