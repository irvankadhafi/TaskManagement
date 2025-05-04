using TaskManagement.Domain.Enums;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.DTOs;

public record UpdateTaskDto(
    TaskStatus? Status = null,
    Priority? Priority = null
);