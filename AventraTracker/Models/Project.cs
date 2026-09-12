namespace AventraTracker.Models;

public class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    // ? = optional, may be null
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    // All tasks that belong to this project
    public List<TaskItem> Tasks { get; set; } = new();
}
