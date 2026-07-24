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

    public async Task AddAsync(TaskItem taskItem)
    {
        _context.Tasks.Add(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(TaskItem taskItem)
    {
        _context.Tasks.Remove(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task LoadCategoryAsync(TaskItem taskItem)
    {
        await _context.Entry(taskItem)
            .Reference(t => t.Category)
            .LoadAsync();
    }

    public async Task UpdateTaskAsync(TaskItem taskItem)
    {
        _context.Tasks.Update(taskItem);
        await _context.SaveChangesAsync();
    }
}