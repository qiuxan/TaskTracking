using TaskTracking.Domain.Entities;

namespace TaskTracking.Application.Categories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategories();
    Task AddCategoryAsync(Category taskItem);
    Task<Category?> GetCategoryByIdAsync(int id);
    Task UpdateCategoryAsync(Category taskItem);
    Task DeleteCategoryAsync(Category taskItem);
}