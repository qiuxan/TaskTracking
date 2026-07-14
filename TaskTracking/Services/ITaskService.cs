using TaskTracking.Dtos;

namespace TaskTracking.Services;

public interface ITaskService
{
    Task<List<TaskResponse>> GetTasksAsync();
    Task<TaskResponse?> GetTaskByIdAsync(int id);
    Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request);
    Task UpdateTaskAsync(int id, UpdateTaskRequest request);
    Task DeleteTaskAsync(int id);
}