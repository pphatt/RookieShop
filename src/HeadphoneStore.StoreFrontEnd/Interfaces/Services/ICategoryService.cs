using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.StoreFrontEnd.Interfaces.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategories(string? searchTerm = null);
    
    Task<List<CategoryDto>> GetAllCategoriesWithSub(string categorySlug);
}
