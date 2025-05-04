using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<Guid> CreateAsync(CreateTaskDto dto);
    Task UpdateAsync(Guid id, UpdateTaskDto dto);
    Task DeleteAsync(Guid id);
    Task<List<TaskDto>> GetAllAsync();
    Task<List<TaskDto>> GetByUserAsync(Guid userId);
}