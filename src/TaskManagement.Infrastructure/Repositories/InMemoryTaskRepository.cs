using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Repositories;

namespace TaskManagement.Infrastructure.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new();

    public Task AddAsync(TaskItem task)
    {
        _tasks.Add(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _tasks.RemoveAll(t => t.Id == id);
        return Task.CompletedTask;
    }

    public Task<List<TaskItem>> GetAllAsync()
    {
        return Task.FromResult(_tasks.ToList());
    }

    public Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
    }

    public Task<List<TaskItem>> GetByUserAsync(Guid userId)
    {
        return Task.FromResult(_tasks.Where(t => t.AssignedUserId == userId).ToList());
    }

    public Task UpdateAsync(TaskItem task)
    {
        var index = _tasks.FindIndex(t => t.Id == task.Id);
        if (index >= 0)
            _tasks[index] = task;
        return Task.CompletedTask;
    }
}