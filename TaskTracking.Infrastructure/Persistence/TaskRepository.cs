using Microsoft.EntityFrameworkCore;
using TaskTracking.Application.Tasks;
using TaskTracking.Domain.Entities;

namespace TaskTracking.Infrastructure.Persistence;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetAllWithCategoryAsync()
    {
        return await _context
            .Tasks
            .Include(t => t.Category)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdWithCategoryAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> CategoryExistsAsync(int categoryId)
    {
        return await _context.Categories.AnyAsync(c => c.Id == categoryId);
    }

    public void Add(TaskItem taskItem)
    {
        _context.Tasks.Add(taskItem);
    }

    public void Remove(TaskItem taskItem)
    {
        _context.Tasks.Remove(taskItem);
    }

    public async Task LoadCategoryAsync(TaskItem taskItem)
    {
        await _context.Entry(taskItem)
            .Reference(t => t.Category)
            .LoadAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}