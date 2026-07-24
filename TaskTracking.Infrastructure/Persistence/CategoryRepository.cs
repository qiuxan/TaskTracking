using Microsoft.EntityFrameworkCore;
using TaskTracking.Application.Categories;
using TaskTracking.Domain.Entities;

namespace TaskTracking.Infrastructure.Persistence;

public class CategoryRepository : ICategoryRepository
{
    private AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }


    public async Task AddAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category taskItem)
    {
        _dbContext.Categories.Remove(taskItem);
        await _dbContext.SaveChangesAsync();
    }
}
