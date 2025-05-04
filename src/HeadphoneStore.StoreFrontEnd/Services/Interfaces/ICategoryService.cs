using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.StoreFrontEnd.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllCategories(string? searchTerm = null);
}
