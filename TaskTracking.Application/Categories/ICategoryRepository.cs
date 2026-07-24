using TaskTracking.Domain.Entities;

namespace TaskTracking.Application.Categories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task AddAsync(Category taskItem);
    Task<Category?> GetByIdAsync(int id);
    Task UpdateAsync(Category taskItem);
    Task DeleteAsync(Category taskItem);
}
