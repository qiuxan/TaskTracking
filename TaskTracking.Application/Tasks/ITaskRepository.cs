using TaskTracking.Domain.Entities;

namespace TaskTracking.Application.Tasks;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllWithCategoryAsync();
    Task<TaskItem?> GetByIdWithCategoryAsync(int id);
    Task<TaskItem?> GetByIdAsync(int id);
    Task<bool> CategoryExistsAsync(int categoryId);
    Task AddAsync(TaskItem taskItem);
    Task RemoveAsync(TaskItem taskItem);
    Task LoadCategoryAsync(TaskItem taskItem);
    Task UpdateTaskAsync(TaskItem taskItem);
}