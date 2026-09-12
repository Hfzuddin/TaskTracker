using AventraTracker.Data;
using AventraTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace AventraTracker.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    // GET /api/tasks/{id}
    [HttpGet("{id}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        // Tasks live inside projects, so flatten all projects' tasks into one sequence and search it
        var task = InMemoryData.Projects
            .SelectMany(p => p.Tasks)
            .FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound();

        return Ok(task);
    }

    // PUT /api/tasks/{id}
    [HttpPut("{id}")]
    public IActionResult Update(int id, TaskItem updated)
    {
        var task = InMemoryData.Projects
            .SelectMany(p => p.Tasks)
            .FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(updated.Title))
            return BadRequest("Title is required.");

        // Only the editable fields change. Id, ProjectId and CreatedAt stay as they are.
        task.Title = updated.Title;
        task.Description = updated.Description;
        task.Status = updated.Status;
        task.DueDate = updated.DueDate;

        return NoContent();
    }

    // DELETE /api/tasks/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // To remove a task we need the project that owns it
        var project = InMemoryData.Projects
            .FirstOrDefault(p => p.Tasks.Any(t => t.Id == id));

        if (project == null)
            return NotFound();

        var task = project.Tasks.First(t => t.Id == id);
        project.Tasks.Remove(task);

        return NoContent();
    }
}
