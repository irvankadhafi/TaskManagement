using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public Priority Priority { get; private set; }
    public TaskStatus Status { get; private set; }
    public Guid AssignedUserId { get; private set; }

    private TaskItem() { } // EF needs this

    public TaskItem(Guid id, string title, string description, DateTime dueDate, Priority priority, Guid assignedUserId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");

        if (dueDate < DateTime.UtcNow)
            throw new ArgumentException("DueDate cannot be in the past.");

        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        Status = TaskStatus.Todo;
        AssignedUserId = assignedUserId;
    }

    public void UpdateStatus(TaskStatus status) => Status = status;
    public void UpdatePriority(Priority priority) => Priority = priority;
}