using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<List<TaskItem>> GetAllAsync();
    Task<List<TaskItem>> GetByUserAsync(Guid userId);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(Guid id);
}