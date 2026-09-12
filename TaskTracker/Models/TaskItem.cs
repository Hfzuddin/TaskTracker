using System.Text.Json.Serialization;

namespace TaskTracker.Models;

// The only allowed states for a task. Used by TaskItem.Status.
// JsonStringEnumConverter: serialise as the name ("InProgress") instead of the number (1),
// for every JSON serialiser (the API responses and the Blazor page's HttpClient).
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskItemStatus
{
    Todo,
    InProgress,
    Done
}

public class TaskItem
{
    public int Id { get; set; }

    // Links this task to its parent project (Project.Id)
    public int ProjectId { get; set; }

    public string Title { get; set; } = "";

    // ? = optional, may be null
    public string? Description { get; set; }

    public TaskItemStatus Status { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAt { get; set; }
}
