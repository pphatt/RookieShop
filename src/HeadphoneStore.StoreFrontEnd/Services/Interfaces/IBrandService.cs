using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.StoreFrontEnd.Services.Interfaces;

public interface IBrandService
{
    Task<List<BrandDto>> GetAllBrands(List<Guid>? categoryIds);
}