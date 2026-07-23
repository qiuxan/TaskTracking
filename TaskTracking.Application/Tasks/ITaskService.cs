using TaskTracking.Application.Tasks.Dtos;

namespace TaskTracking.Application.Tasks;

public interface ITaskService
{
    Task<List<TaskResponse>> GetTasksAsync();
    Task<TaskResponse?> GetTaskByIdAsync(int id);
    Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request);
    Task UpdateTaskAsync(int id, UpdateTaskRequest request);
    Task DeleteTaskAsync(int id);
}