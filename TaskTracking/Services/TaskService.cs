using Microsoft.EntityFrameworkCore;
using TaskTracking.Application.Tasks;
using TaskTracking.Application.Tasks.Dtos;
using TaskTracking.Domain.Entities;
using TaskTracking.Infrastructure.Persistence;

namespace TaskTracking.Services;

public class TaskService:ITaskService
{
    private readonly AppDbContext _context;
    
    public TaskService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<TaskResponse>> GetTasksAsync()
    {
       var taskItems = await _context.Tasks
           .Include(t => t.Category)
           .ToListAsync();
       
       var response = taskItems.Select(MapTaskItemToTaskResponse).ToList();
       
       return response;
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(int id)
    {
        var taskItem = await _context.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (taskItem is null)  return null;

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

        _context.Tasks.Add(taskItem);
        await _context.SaveChangesAsync();

        if (taskItem.CategoryId is not null)
            await _context.Entry(taskItem).Reference(t => t.Category).LoadAsync();

        return MapTaskItemToTaskResponse(taskItem);
    }

    public async Task UpdateTaskAsync(int id, UpdateTaskRequest request)
    {
        var taskExisting = await _context.Tasks.FirstOrDefaultAsync(t=>t.Id == id);
        
        if(taskExisting is null)
            throw new KeyNotFoundException($"Task with id {id} not found");
        
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("Title is required");

        if (request.CategoryId is not null)
            await EnsureCategoryExistsAsync(request.CategoryId.Value);
        
        taskExisting.Title = request.Title.Trim();
        taskExisting.IsCompleted = request.IsCompleted;
        taskExisting.CategoryId = request.CategoryId;
        
        await _context.SaveChangesAsync();

    }

    public async Task DeleteTaskAsync(int id)
    {
        var existingTaskItem = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        if (existingTaskItem is null)
            throw new KeyNotFoundException($"Task with id {id} not found");
        _context.Tasks.Remove(existingTaskItem);
        await _context.SaveChangesAsync();
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
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == categoryId);

        if (!categoryExists)
            throw new ArgumentException($"Category with id {categoryId} does not exist");
    }
}
