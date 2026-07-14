using Microsoft.EntityFrameworkCore;
using TaskTracking.Data;
using TaskTracking.Dtos;
using TaskTracking.Models;

namespace TaskTracking.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryResponse>> GetCategoriesAsync()
    {
        var categories = await _context.Categories.ToListAsync();

        return categories.Select(MapCategoryToCategoryResponse).ToList();
    }

    public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

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

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return MapCategoryToCategoryResponse(category);
    }

    public async Task UpdateCategoryAsync(int id, UpdateCategoryRequest request)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            throw new KeyNotFoundException($"Category with id {id} not found");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required");

        category.Name = request.Name.Trim();

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            throw new KeyNotFoundException($"Category with id {id} not found");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
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
