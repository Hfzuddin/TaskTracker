# Task Tracker API

A small Task Tracker REST API built with **.NET 8 Web API** and **Swagger**, for the Junior Full Stack Developer take-home assignment.

- **Project** = a folder (e.g. *Website Redesign*)
- **Task** = an item inside a project (e.g. *Create wireframes*, status: Todo)

Data is stored in a hard-coded in-memory `List` — no database. Data resets every time the app restarts.

---

## How to run

**Requirements:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or newer.

```bash
cd TaskTracker
dotnet run --launch-profile http
```

Then open **http://localhost:5041/swagger** in your browser.

Swagger UI lists every endpoint. For each one: expand → **Try it out** → fill in → **Execute**.

> Note: the root URL `http://localhost:5041/` returns 404 — this is an API with no home page. Use `/swagger`.

---

## What is completed

| Requirement | Status |
|---|---|
| .NET 8 Web API with Swagger | ✅ Done |
| In-memory hard-coded `List` with sample data (2 projects, 4 tasks) | ✅ Done |
| Project CRUD — list, get one, create, update, delete | ✅ Done |
| Task CRUD — list by project, get one, create under project, update, delete | ✅ Done |
| Project ↔ Task relationship | ✅ Done |
| README | ✅ Done |
| **Optional:** Database (SQL Server / SQLite) | ❌ Not done |
| **Optional:** Blazor page calling the API | ❌ Not done |

All 10 endpoints have been tested through Swagger, including the error cases (404 for missing ids, 400 for invalid input).

---

## Endpoints

| | Projects | Tasks |
|---|---|---|
| **List** | `GET /api/projects` | `GET /api/projects/{id}/tasks` |
| **Get one** | `GET /api/projects/{id}` | `GET /api/tasks/{id}` |
| **Create** | `POST /api/projects` | `POST /api/projects/{id}/tasks` |
| **Update** | `PUT /api/projects/{id}` | `PUT /api/tasks/{id}` |
| **Delete** | `DELETE /api/projects/{id}` | `DELETE /api/tasks/{id}` |

### Status codes used

| Code | When |
|---|---|
| `200 OK` | GET succeeded, body contains the data |
| `201 Created` | POST succeeded; `Location` header points to the new item |
| `204 No Content` | PUT / DELETE succeeded; nothing to return |
| `400 Bad Request` | Missing `name` / `title`, invalid `status` value, or malformed JSON |
| `404 Not Found` | No project / task with that id |

### Example request bodies

Create a project — `POST /api/projects`
```json
{ "name": "Office Move", "description": "Relocate to new HQ" }
```

Create a task — `POST /api/projects/1/tasks`
```json
{ "title": "Book movers", "description": null, "status": "Todo", "dueDate": "2026-10-15" }
```

Update a task — `PUT /api/tasks/1`
```json
{ "title": "Create wireframes", "description": "Approved", "status": "Done", "dueDate": null }
```

`status` must be one of `Todo`, `InProgress`, `Done`.

`id`, `createdAt` and `projectId` are set by the server and ignored if sent by the client.

---

## Models

**Project:** `Id`, `Name`, `Description?`, `CreatedAt`, `Tasks`

**TaskItem:** `Id`, `ProjectId`, `Title`, `Description?`, `Status` (`Todo` / `InProgress` / `Done`), `DueDate?`, `CreatedAt`

`?` = optional (may be `null`).

### Sample data (loaded on startup)

| Project | Tasks |
|---|---|
| 1 — Website Redesign | 1 — Create wireframes (Todo), 2 — Choose colour palette (InProgress) |
| 2 — Mobile App Launch | 3 — Set up app store accounts (Done), 4 — Write release notes (Todo) |

---

## Project structure

```
TaskTracker/
├── TaskTracker.csproj        Project file — targets net8.0, references Swashbuckle (Swagger)
├── Program.cs                   App startup — registers controllers + Swagger, enum-as-string JSON
├── Models/
│   ├── Project.cs               Project class
│   └── TaskItem.cs              TaskItemStatus enum + TaskItem class
├── Data/
│   └── InMemoryData.cs          Static List<Project> with sample data + Id counters
└── Controllers/
    ├── ProjectsController.cs    /api/projects endpoints (+ the nested /api/projects/{id}/tasks)
    └── TasksController.cs       /api/tasks/{id} endpoints
```

---

## Design notes

- **Tasks are stored inside their project** (`Project.Tasks`), not in a separate list, so a task exists in exactly one place and nothing can get out of sync. Deleting a project removes its tasks automatically. Looking up a task by id flattens all projects' tasks with `SelectMany`.
- **The server owns `Id`, `CreatedAt` and `ProjectId`.** On create, `ProjectId` is taken from the URL, never from the request body. On update, only the editable fields are copied.
- **`Status` is an enum** (`TaskItemStatus`). Invalid values are rejected automatically with 400, and Swagger shows a dropdown. `JsonStringEnumConverter` makes it appear as text (`"Done"`) instead of a number.
- The enum is named `TaskItemStatus` rather than `TaskStatus` to avoid a clash with .NET's built-in `System.Threading.Tasks.TaskStatus`.
- No database, repository layer, or dependency injection — kept deliberately simple as per the assignment.
