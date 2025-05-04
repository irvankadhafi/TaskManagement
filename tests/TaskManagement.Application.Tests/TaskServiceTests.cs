using FluentAssertions;
using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Repositories;
using TaskStatus = TaskManagement.Domain.Enums.TaskStatus;

namespace TaskManagement.Application.Tests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepo;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        _mockRepo = new Mock<ITaskRepository>();
        _taskService = new TaskService(_mockRepo.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidInput_ReturnsGuid()
    {
        // Arrange
        var dto = new CreateTaskDto("Test", "Desc", DateTime.UtcNow.AddDays(1), Priority.Medium, Guid.NewGuid());

        // Act
        var result = await _taskService.CreateAsync(dto);

        // Assert
        result.Should().NotBeEmpty();
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidTask_UpdatesSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var task = new TaskItem(id, "Test", "Desc", DateTime.UtcNow.AddDays(1), Priority.Low, Guid.NewGuid());
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(task);

        var dto = new UpdateTaskDto(TaskStatus.Done, Priority.High);

        // Act
        await _taskService.UpdateAsync(id, dto);

        // Assert
        task.Status.Should().Be(TaskStatus.Done);
        task.Priority.Should().Be(Priority.High);
        _mockRepo.Verify(r => r.UpdateAsync(task), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_TaskNotFound_ThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskItem?)null);
        var dto = new UpdateTaskDto(TaskStatus.Done, Priority.High);

        // Act
        var act = () => _taskService.UpdateAsync(id, dto);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("*not found*");
    }
    
    [Fact]
    public async Task CreateAsync_WithPastDueDate_ThrowsException()
    {
        var dto = new CreateTaskDto("Title", "Desc", DateTime.UtcNow.AddDays(-1), Priority.Low, Guid.NewGuid());

        Func<Task> act = async () => await _taskService.CreateAsync(dto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*DueDate cannot be in the past*");

        _mockRepo.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Never);
    }
    
    [Fact]
    public async Task GetAllAsync_WhenNoTasks_ReturnsEmptyList()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TaskItem>());

        var result = await _taskService.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetByUserAsync_ReturnsOnlyTasksAssignedToUser()
    {
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var list = new List<TaskItem>
        {
            new(Guid.NewGuid(), "T1", "D", DateTime.UtcNow.AddDays(1), Priority.Medium, userId),
            new(Guid.NewGuid(), "T2", "D", DateTime.UtcNow.AddDays(1), Priority.Low, otherUserId)
        };

        _mockRepo.Setup(r => r.GetByUserAsync(userId)).ReturnsAsync(list.Where(t => t.AssignedUserId == userId).ToList());

        var result = await _taskService.GetByUserAsync(userId);

        result.Should().HaveCount(1);
        result.First().AssignedUserId.Should().Be(userId);
    }
    
    [Fact]
    public async Task DeleteAsync_DeletesTask()
    {
        var id = Guid.NewGuid();

        await _taskService.DeleteAsync(id);

        _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
    }


}
