# Task Tracker API

A small Task Tracker REST API built with **.NET 8 Web API** and **Swagger**, for the Junior Full Stack Developer take-home assignment.

- **Project** = a folder (e.g. *Website Redesign*)
- **Task** = an item inside a project (e.g. *Create wireframes*, status: Todo)

Data is stored in a hard-coded in-memory `List` — no database. Data resets every time the app restarts.

---

## How to run

**Requirements:** [Git](https://git-scm.com/downloads) and the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or newer. Check with `dotnet --version`.

```bash
# 1. Get the code
git clone https://github.com/Hfzuddin/TaskTracker.git

# 2. Go into the project folder (the repo folder contains a TaskTracker project folder)
cd TaskTracker/TaskTracker

# 3. Run — restores NuGet packages, builds and starts the server
dotnet run --launch-profile http
```

Wait for `Now listening on: http://localhost:5041`. Press `Ctrl+C` to stop.

If you downloaded the repo as a ZIP instead of cloning, unzip it and run steps 2–3 from inside the unzipped folder.

Then open in your browser:

| URL | What it is |
|---|---|
| **http://localhost:5041/swagger** | Swagger UI — lists every endpoint. Expand → **Try it out** → fill in → **Execute**. |
| **http://localhost:5041/** | Blazor page (optional bonus) — a simple UI that calls the API: add/delete projects and tasks, change task status. |

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
| **Optional:** Blazor page calling the API | ✅ Done — `Components/Pages/Home.razor`, served at `/` |

All 10 endpoints have been tested through Swagger, including the error cases (404 for missing ids, 400 for invalid input). The Blazor page has been tested in the browser for every action.

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


## Design notes

- **Tasks are stored inside their project** (`Project.Tasks`), not in a separate list, so a task exists in exactly one place and nothing can get out of sync. Deleting a project removes its tasks automatically. Looking up a task by id flattens all projects' tasks with `SelectMany`.
- **The server owns `Id`, `CreatedAt` and `ProjectId`.** On create, `ProjectId` is taken from the URL, never from the request body. On update, only the editable fields are copied.
- **`Status` is an enum** (`TaskItemStatus`). Invalid values are rejected automatically with 400, and Swagger shows a dropdown. A `[JsonConverter(typeof(JsonStringEnumConverter))]` attribute on the enum makes it appear as text (`"Done"`) instead of a number in every serialiser (API responses and the Blazor page's `HttpClient`).
- The enum is named `TaskItemStatus` rather than `TaskStatus` to avoid a clash with .NET's built-in `System.Threading.Tasks.TaskStatus`.
- **Blazor page (optional bonus)** lives in the same project as the API, using Blazor Server interactive mode. It does **not** touch `InMemoryData` directly — it calls the REST endpoints through `HttpClient`, exactly like an external client would, and re-fetches `GET /api/projects` after every change so the API stays the single source of truth.
- No database, repository layer, or custom services — kept deliberately simple as per the assignment.
