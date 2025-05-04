using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.DTOs;

public record TaskDto(
    Guid Id,
    string Title,
    string Description,
    DateTime DueDate,
    Priority Priority,
    TaskStatus Status,
    Guid AssignedUserId
)
{
    public TaskDto(TaskItem task) : this(
        task.Id,
        task.Title,
        task.Description,
        task.DueDate,
        task.Priority,
        task.Status,
        task.AssignedUserId
    ) { }
}