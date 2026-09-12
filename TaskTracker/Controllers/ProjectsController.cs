using TaskTracker.Data;
using TaskTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{

//PROJECT

    // GET /api/projects
    [HttpGet]
    public ActionResult<List<Project>> GetAll()
    {
        return Ok(InMemoryData.Projects);//from controller base
    }

    // GET /api/projects/{id}
    [HttpGet("{id}")]
    public ActionResult<Project> GetById(int id)
    {
        var project = InMemoryData.Projects.FirstOrDefault(p => p.Id == id);

        if (project == null)
            return NotFound();//from controller base

        return Ok(project);
    }

    // POST /api/projects
    [HttpPost]
    public ActionResult<Project> Create(Project project)
    {
        if (string.IsNullOrWhiteSpace(project.Name))
            return BadRequest("Name is required.");

        // The server decides these, not the client
        project.Id = InMemoryData.NextProjectId();
        project.CreatedAt = DateTime.UtcNow;
        project.Tasks = new();   // tasks are added via POST /api/projects/{id}/tasks

        InMemoryData.Projects.Add(project);

        // 201 Created + Location header pointing to GET /api/projects/{id} + the new project as body
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    // PUT /api/projects/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, Project updated)
    {
        var project = InMemoryData.Projects.FirstOrDefault(p => p.Id == id);
        if (project == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(updated.Name))
            return BadRequest("Name is required.");

        // Only the editable fields change. Id, CreatedAt and Tasks stay as they are.
        project.Name = updated.Name;
        project.Description = updated.Description;

        return NoContent();   // 204: success, nothing to return
    }

    // DELETE /api/projects/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var project = InMemoryData.Projects.FirstOrDefault(p => p.Id == id);
        if (project == null)
            return NotFound();

        InMemoryData.Projects.Remove(project);   // its tasks go with it (they're nested inside)
        return NoContent();
    }


// TASK

    // GET /api/projects/{id}/tasks
    [HttpGet("{id}/tasks")]
    public ActionResult<List<TaskItem>> GetTasks(int id)
    {
        var project = InMemoryData.Projects.FirstOrDefault(p => p.Id == id);
        if (project == null)
            return NotFound();

        return Ok(project.Tasks);
    }

    // POST /api/projects/{id}/tasks
    [HttpPost("{id}/tasks")]
    public ActionResult<TaskItem> CreateTask(int id, TaskItem task)
    {
        var project = InMemoryData.Projects.FirstOrDefault(p => p.Id == id);
        if (project == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(task.Title))
            return BadRequest("Title is required.");

        // The server decides these, not the client
        task.Id = InMemoryData.NextTaskId();
        task.ProjectId = id;              // taken from the URL, never from the body
        task.CreatedAt = DateTime.UtcNow;

        project.Tasks.Add(task);

        // 201 Created; the new task's own URL will be GET /api/tasks/{id} (Step 8)
        return Created($"/api/tasks/{task.Id}", task);
    }
}
