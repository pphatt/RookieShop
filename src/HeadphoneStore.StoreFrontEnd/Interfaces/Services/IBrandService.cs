using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.StoreFrontEnd.Interfaces.Services;

public interface IBrandService
{
    Task<List<BrandDto>> GetAllBrands(List<Guid>? categoryIds);
}