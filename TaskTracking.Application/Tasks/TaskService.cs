using TaskTracking.Application.Tasks.Dtos;
using TaskTracking.Domain.Entities;

namespace TaskTracking.Application.Tasks;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<List<TaskResponse>> GetTasksAsync()
    {
        var taskItems = await _taskRepository.GetAllWithCategoryAsync();
        var response = taskItems.Select(MapTaskItemToTaskResponse).ToList();
        return response;
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(int id)
    {
        var taskItem = await _taskRepository.GetByIdWithCategoryAsync(id);

        if (taskItem is null) return null;

        var response = MapTaskItemToTaskResponse(taskItem);

        return response;
    }

    public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title is required");

        if (request.CategoryId is not null)
            await EnsureCategoryExistsAsync(request.CategoryId.Value);

        var taskItem = new TaskItem
        {
            IsCompleted = false,
            Title = request.Title.Trim(),
            CreatedAt = DateTime.UtcNow,
            CategoryId = request.CategoryId
        };

        _taskRepository.Add(taskItem);
        await _taskRepository.SaveChangesAsync();

        if (taskItem.CategoryId is not null)
            await _taskRepository.LoadCategoryAsync(taskItem);

        return MapTaskItemToTaskResponse(taskItem);
    }

    public async Task UpdateTaskAsync(int id, UpdateTaskRequest request)
    {
        var taskExisting = await _taskRepository.GetByIdAsync(id);

        if (taskExisting is null)
            throw new KeyNotFoundException($"Task with id {id} not found");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title is required");

        if (request.CategoryId is not null)
            await EnsureCategoryExistsAsync(request.CategoryId.Value);

        taskExisting.Title = request.Title.Trim();
        taskExisting.IsCompleted = request.IsCompleted;
        taskExisting.CategoryId = request.CategoryId;

        await _taskRepository.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var existingTaskItem = await _taskRepository.GetByIdAsync(id);
        if (existingTaskItem is null)
            throw new KeyNotFoundException($"Task with id {id} not found");
        _taskRepository.Remove(existingTaskItem);
        await _taskRepository.SaveChangesAsync();
    }

    private static TaskResponse MapTaskItemToTaskResponse(TaskItem item)
    {
        return new TaskResponse
        {
            Id = item.Id,
            Title = item.Title,
            CreatedAt = item.CreatedAt,
            IsCompleted = item.IsCompleted,
            CategoryId = item.CategoryId,
            CategoryName = item.Category?.Name
        };
    }

    private async Task EnsureCategoryExistsAsync(int categoryId)
    {
        var categoryExists = await _taskRepository.CategoryExistsAsync(categoryId);

        if (!categoryExists)
            throw new ArgumentException($"Category with id {categoryId} does not exist");
    }
}