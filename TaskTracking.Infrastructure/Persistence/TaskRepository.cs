using TaskTracking.Application.Tasks;
using TaskTracking.Domain.Entities;

namespace TaskTracking.Infrastructure.Persistence;

public class TaskRepository:ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository( AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<TaskItem>> GetAllWithCategoryAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<TaskItem?> GetByIdWithCategoryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(TaskItem taskItem)
    {
        throw new NotImplementedException();
    }

    public void Remove(TaskItem taskItem)
    {
        throw new NotImplementedException();
    }

    public async Task LoadCategoryAsync(TaskItem taskItem)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}