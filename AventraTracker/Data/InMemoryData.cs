using AventraTracker.Models;

namespace AventraTracker.Data;

// All data lives here, in memory, for as long as the app is running.
// Restarting the app resets it to this sample data.
public static class InMemoryData
{
    public static List<Project> Projects { get; } = new()
    {
        new Project
        {
            Id = 1,
            Name = "Website Redesign",
            Description = "Refresh the company website with a modern look",
            CreatedAt = new DateTime(2026, 9, 1),
            Tasks = new()
            {
                new TaskItem
                {
                    Id = 1,
                    ProjectId = 1,
                    Title = "Create wireframes",
                    Description = "Low-fidelity wireframes for home and about pages",
                    Status = TaskItemStatus.Todo,
                    DueDate = new DateTime(2026, 9, 20),
                    CreatedAt = new DateTime(2026, 9, 1)
                },
                new TaskItem
                {
                    Id = 2,
                    ProjectId = 1,
                    Title = "Choose colour palette",
                    Description = null,
                    Status = TaskItemStatus.InProgress,
                    DueDate = null,
                    CreatedAt = new DateTime(2026, 9, 2)
                }
            }
        },
        new Project
        {
            Id = 2,
            Name = "Mobile App Launch",
            Description = null,
            CreatedAt = new DateTime(2026, 9, 5),
            Tasks = new()
            {
                new TaskItem
                {
                    Id = 3,
                    ProjectId = 2,
                    Title = "Set up app store accounts",
                    Description = "Apple App Store and Google Play",
                    Status = TaskItemStatus.Done,
                    DueDate = new DateTime(2026, 9, 10),
                    CreatedAt = new DateTime(2026, 9, 5)
                },
                new TaskItem
                {
                    Id = 4,
                    ProjectId = 2,
                    Title = "Write release notes",
                    Description = null,
                    Status = TaskItemStatus.Todo,
                    DueDate = new DateTime(2026, 9, 30),
                    CreatedAt = new DateTime(2026, 9, 6)
                }
            }
        }
    };

    // Counters for generating unique Ids for new items.
    // They start after the highest Id in the sample data above.
    private static int _nextProjectId = 3;
    private static int _nextTaskId = 5;

    public static int NextProjectId() => _nextProjectId++;
    public static int NextTaskId() => _nextTaskId++;
}
