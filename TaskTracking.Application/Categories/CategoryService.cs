using TaskTracking.Application.Categories.Dtos;
using TaskTracking.Domain.Entities;

namespace TaskTracking.Application.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryResponse>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();

        return categories.Select(MapCategoryToCategoryResponse).ToList();
    }

    public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is null)
            return null;

        return MapCategoryToCategoryResponse(category);
    }

    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required");

        var category = new Category
        {
            Name = request.Name.Trim()
        };

        await _categoryRepository.AddAsync(category);

        return MapCategoryToCategoryResponse(category);
    }

    public async Task UpdateCategoryAsync(int id, UpdateCategoryRequest request)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is null)
            throw new KeyNotFoundException($"Category with id {id} not found");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required");

        category.Name = request.Name.Trim();

        await _categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is null)
            throw new KeyNotFoundException($"Category with id {id} not found");

        await _categoryRepository.DeleteAsync(category);
    }

    private static CategoryResponse MapCategoryToCategoryResponse(Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
