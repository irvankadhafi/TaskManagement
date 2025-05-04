using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repo;

    public TaskService(ITaskRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> CreateAsync(CreateTaskDto dto)
    {
        var task = new TaskItem(
            Guid.NewGuid(),
            dto.Title,
            dto.Description,
            dto.DueDate,
            dto.Priority,
            dto.AssignedUserId
        );

        await _repo.AddAsync(task);
        return task.Id;
    }

    public async Task UpdateAsync(Guid id, UpdateTaskDto dto)
    {
        var task = await _repo.GetByIdAsync(id) ?? throw new Exception("Task not found");

        if (dto.Status.HasValue)
            task.UpdateStatus(dto.Status.Value);
        if (dto.Priority.HasValue)
            task.UpdatePriority(dto.Priority.Value);

        await _repo.UpdateAsync(task);
    }

    public Task DeleteAsync(Guid id) => _repo.DeleteAsync(id);
    public async Task<List<TaskDto>> GetAllAsync()
        => (await _repo.GetAllAsync()).Select(t => new TaskDto(t)).ToList();

    public async Task<List<TaskDto>> GetByUserAsync(Guid userId)
        => (await _repo.GetByUserAsync(userId)).Select(t => new TaskDto(t)).ToList();
}