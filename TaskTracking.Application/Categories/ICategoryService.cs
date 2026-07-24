using TaskTracking.Application.Categories.Dtos;

namespace TaskTracking.Application.Categories;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetCategoriesAsync();
    Task<CategoryResponse?> GetCategoryByIdAsync(int id);
    Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request);
    Task UpdateCategoryAsync(int id, UpdateCategoryRequest request);
    Task DeleteCategoryAsync(int id);
}
