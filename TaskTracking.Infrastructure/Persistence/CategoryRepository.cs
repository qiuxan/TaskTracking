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

    public async Task<List<Category>> GetAllCategories()
    {
        return await _dbContext.Categories.ToListAsync();
    }


    public async Task AddCategoryAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(Category taskItem)
    {
        _dbContext.Categories.Remove(taskItem);
        await _dbContext.SaveChangesAsync();
    }
}